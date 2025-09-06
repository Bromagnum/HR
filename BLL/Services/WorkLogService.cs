using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services
{
    public class WorkLogService : IWorkLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // Company policies (can be moved to configuration)
        private readonly TimeSpan StandardWorkStart = new(9, 0, 0); // 09:00
        private readonly TimeSpan StandardWorkEnd = new(18, 0, 0);   // 18:00
        private readonly decimal StandardWorkHours = 8.0m;
        private readonly TimeSpan LateArrivalThreshold = new(9, 15, 0); // 09:15

        public WorkLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<WorkLogListDto>>> GetAllAsync()
        {
            try
            {
                var workLogs = await _unitOfWork.WorkLogs.GetAllAsync();
                var workLogDtos = _mapper.Map<IEnumerable<WorkLogListDto>>(workLogs);
                return Result<IEnumerable<WorkLogListDto>>.Ok(workLogDtos, "Çalışma kayıtları başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<WorkLogListDto>>.Fail($"Çalışma kayıtları getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<WorkLogDetailDto>> GetByIdAsync(int id)
        {
            try
            {
                var workLog = await _unitOfWork.WorkLogs.GetByIdAsync(id);
                if (workLog == null)
                {
                    return Result<WorkLogDetailDto>.Fail("Çalışma kaydı bulunamadı.");
                }

                var workLogDto = _mapper.Map<WorkLogDetailDto>(workLog);
                return Result<WorkLogDetailDto>.Ok(workLogDto, "Çalışma kaydı başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return Result<WorkLogDetailDto>.Fail($"Çalışma kaydı getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<WorkLogDetailDto>> CreateAsync(WorkLogCreateDto dto)
        {
            try
            {
                // Check if person exists
                var person = await _unitOfWork.Persons.GetByIdAsync(dto.PersonId);
                if (person == null)
                {
                    return Result<WorkLogDetailDto>.Fail("Belirtilen personel bulunamadı.");
                }

                // Check if work log already exists for this person and date
                var existingWorkLog = await _unitOfWork.WorkLogs.GetByPersonIdAndDateAsync(dto.PersonId, dto.Date);
                if (existingWorkLog != null)
                {
                    return Result<WorkLogDetailDto>.Fail("Bu tarih için zaten bir çalışma kaydı mevcut.");
                }

                var workLog = _mapper.Map<WorkLog>(dto);
                workLog.CreatedAt = DateTime.Now;
                workLog.IsActive = true;

                // Calculate time-related fields
                CalculateWorkLogTimes(workLog);

                await _unitOfWork.WorkLogs.AddAsync(workLog);
                await _unitOfWork.SaveChangesAsync();

                var workLogDto = _mapper.Map<WorkLogDetailDto>(workLog);
                return Result<WorkLogDetailDto>.Ok(workLogDto, "Çalışma kaydı başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                return Result<WorkLogDetailDto>.Fail($"Çalışma kaydı oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<WorkLogDetailDto>> UpdateAsync(WorkLogUpdateDto dto)
        {
            try
            {
                var workLog = await _unitOfWork.WorkLogs.GetByIdAsync(dto.Id);
                if (workLog == null)
                {
                    return Result<WorkLogDetailDto>.Fail("Çalışma kaydı bulunamadı.");
                }

                // Map updates
                _mapper.Map(dto, workLog);
                workLog.UpdatedAt = DateTime.Now;

                // Recalculate time-related fields
                CalculateWorkLogTimes(workLog);

                _unitOfWork.WorkLogs.Update(workLog);
                await _unitOfWork.SaveChangesAsync();

                var workLogDto = _mapper.Map<WorkLogDetailDto>(workLog);
                return Result<WorkLogDetailDto>.Ok(workLogDto, "Çalışma kaydı başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                return Result<WorkLogDetailDto>.Fail($"Çalışma kaydı güncellenirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var workLog = await _unitOfWork.WorkLogs.GetByIdAsync(id);
                if (workLog == null)
                {
                    return Result.Fail("Çalışma kaydı bulunamadı.");
                }

                _unitOfWork.WorkLogs.Remove(workLog);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok("Çalışma kaydı başarıyla silindi.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Çalışma kaydı silinirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result> ToggleStatusAsync(int id)
        {
            try
            {
                var workLog = await _unitOfWork.WorkLogs.GetByIdAsync(id);
                if (workLog == null)
                {
                    return Result.Fail("Çalışma kaydı bulunamadı.");
                }

                workLog.IsActive = !workLog.IsActive;
                workLog.UpdatedAt = DateTime.Now;

                _unitOfWork.WorkLogs.Update(workLog);
                await _unitOfWork.SaveChangesAsync();

                string statusText = workLog.IsActive ? "aktif" : "pasif";
                return Result.Ok($"Çalışma kaydı {statusText} duruma getirildi.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Çalışma kaydı durumu değiştirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<WorkLogDetailDto>> CheckInAsync(WorkLogCheckInDto dto)
        {
            try
            {
                var today = DateTime.Today;

                // Check if already checked in today
                var existingWorkLog = await _unitOfWork.WorkLogs.GetByPersonIdAndDateAsync(dto.PersonId, today);
                if (existingWorkLog != null)
                {
                    return Result<WorkLogDetailDto>.Fail("Bugün için zaten giriş kaydı mevcut.");
                }

                var workLog = new WorkLog
                {
                    PersonId = dto.PersonId,
                    Date = today,
                    StartTime = dto.StartTime,
                    WorkType = dto.WorkType,
                    Location = dto.Location,
                    Notes = dto.Notes,
                    Status = "Active",
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    IsWeekend = today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday,
                    IsLateArrival = dto.StartTime > LateArrivalThreshold
                };

                await _unitOfWork.WorkLogs.AddAsync(workLog);
                await _unitOfWork.SaveChangesAsync();

                var workLogDto = _mapper.Map<WorkLogDetailDto>(workLog);
                return Result<WorkLogDetailDto>.Ok(workLogDto, "Giriş kaydı başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                return Result<WorkLogDetailDto>.Fail($"Giriş kaydı oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<WorkLogDetailDto>> CheckOutAsync(WorkLogCheckOutDto dto)
        {
            try
            {
                var workLog = await _unitOfWork.WorkLogs.GetByIdAsync(dto.Id);
                if (workLog == null)
                {
                    return Result<WorkLogDetailDto>.Fail("Çalışma kaydı bulunamadı.");
                }

                if (workLog.EndTime.HasValue)
                {
                    return Result<WorkLogDetailDto>.Fail("Bu kayıt için çıkış işlemi zaten yapılmış.");
                }

                workLog.EndTime = dto.EndTime;
                workLog.TasksCompleted = dto.TasksCompleted;
                if (!string.IsNullOrEmpty(dto.Notes))
                {
                    workLog.Notes = string.IsNullOrEmpty(workLog.Notes) ? dto.Notes : $"{workLog.Notes}\n{dto.Notes}";
                }
                workLog.Status = "Completed";
                workLog.UpdatedAt = DateTime.Now;

                // Check for early departure
                workLog.IsEarlyDeparture = dto.EndTime < StandardWorkEnd;

                // Calculate time-related fields
                CalculateWorkLogTimes(workLog);

                _unitOfWork.WorkLogs.Update(workLog);
                await _unitOfWork.SaveChangesAsync();

                var workLogDto = _mapper.Map<WorkLogDetailDto>(workLog);
                return Result<WorkLogDetailDto>.Ok(workLogDto, "Çıkış kaydı başarıyla tamamlandı.");
            }
            catch (Exception ex)
            {
                return Result<WorkLogDetailDto>.Fail($"Çıkış kaydı oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<WorkLogDetailDto>> StartBreakAsync(int workLogId)
        {
            try
            {
                var workLog = await _unitOfWork.WorkLogs.GetByIdAsync(workLogId);
                if (workLog == null)
                {
                    return Result<WorkLogDetailDto>.Fail("Çalışma kaydı bulunamadı.");
                }

                if (workLog.BreakStartTime.HasValue)
                {
                    return Result<WorkLogDetailDto>.Fail("Mola zaten başlatılmış.");
                }

                workLog.BreakStartTime = DateTime.Now.TimeOfDay;
                workLog.UpdatedAt = DateTime.Now;

                _unitOfWork.WorkLogs.Update(workLog);
                await _unitOfWork.SaveChangesAsync();

                var workLogDto = _mapper.Map<WorkLogDetailDto>(workLog);
                return Result<WorkLogDetailDto>.Ok(workLogDto, "Mola başlatıldı.");
            }
            catch (Exception ex)
            {
                return Result<WorkLogDetailDto>.Fail($"Mola başlatılırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<WorkLogDetailDto>> EndBreakAsync(int workLogId)
        {
            try
            {
                var workLog = await _unitOfWork.WorkLogs.GetByIdAsync(workLogId);
                if (workLog == null)
                {
                    return Result<WorkLogDetailDto>.Fail("Çalışma kaydı bulunamadı.");
                }

                if (!workLog.BreakStartTime.HasValue)
                {
                    return Result<WorkLogDetailDto>.Fail("Mola başlatılmamış.");
                }

                if (workLog.BreakEndTime.HasValue)
                {
                    return Result<WorkLogDetailDto>.Fail("Mola zaten bitirilmiş.");
                }

                workLog.BreakEndTime = DateTime.Now.TimeOfDay;
                workLog.UpdatedAt = DateTime.Now;

                // Calculate break duration
                var breakDuration = workLog.BreakEndTime.Value - workLog.BreakStartTime.Value;
                workLog.BreakDurationMinutes = (decimal)breakDuration.TotalMinutes;

                // Recalculate total hours if end time is set
                if (workLog.EndTime.HasValue)
                {
                    CalculateWorkLogTimes(workLog);
                }

                _unitOfWork.WorkLogs.Update(workLog);
                await _unitOfWork.SaveChangesAsync();

                var workLogDto = _mapper.Map<WorkLogDetailDto>(workLog);
                return Result<WorkLogDetailDto>.Ok(workLogDto, "Mola bitirildi.");
            }
            catch (Exception ex)
            {
                return Result<WorkLogDetailDto>.Fail($"Mola bitirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<WorkLogListDto>>> GetByPersonIdAsync(int personId)
        {
            try
            {
                var workLogs = await _unitOfWork.WorkLogs.GetByPersonIdAsync(personId);
                var workLogDtos = _mapper.Map<IEnumerable<WorkLogListDto>>(workLogs);
                return Result<IEnumerable<WorkLogListDto>>.Ok(workLogDtos, "Personel çalışma kayıtları başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<WorkLogListDto>>.Fail($"Personel çalışma kayıtları getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<WorkLogDetailDto>> GetTodayWorkLogAsync(int personId)
        {
            try
            {
                var today = DateTime.Today;
                var workLog = await _unitOfWork.WorkLogs.GetByPersonIdAndDateAsync(personId, today);
                
                if (workLog == null)
                {
                    return Result<WorkLogDetailDto>.Fail("Bugün için çalışma kaydı bulunamadı.");
                }

                var workLogDto = _mapper.Map<WorkLogDetailDto>(workLog);
                return Result<WorkLogDetailDto>.Ok(workLogDto, "Günlük çalışma kaydı başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return Result<WorkLogDetailDto>.Fail($"Günlük çalışma kaydı getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<WorkLogDetailDto>> GetActiveWorkLogAsync(int personId)
        {
            try
            {
                var activeWorkLogs = await _unitOfWork.WorkLogs.GetActiveWorkLogsAsync();
                var activeWorkLog = activeWorkLogs.FirstOrDefault(w => w.PersonId == personId);
                
                if (activeWorkLog == null)
                {
                    return Result<WorkLogDetailDto>.Fail("Aktif çalışma kaydı bulunamadı.");
                }

                var workLogDto = _mapper.Map<WorkLogDetailDto>(activeWorkLog);
                return Result<WorkLogDetailDto>.Ok(workLogDto, "Aktif çalışma kaydı başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return Result<WorkLogDetailDto>.Fail($"Aktif çalışma kaydı getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<WorkLogListDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var workLogs = await _unitOfWork.WorkLogs.GetByDateRangeAsync(startDate, endDate);
                var workLogDtos = _mapper.Map<IEnumerable<WorkLogListDto>>(workLogs);
                return Result<IEnumerable<WorkLogListDto>>.Ok(workLogDtos, "Tarih aralığı çalışma kayıtları başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<WorkLogListDto>>.Fail($"Tarih aralığı çalışma kayıtları getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<WorkLogListDto>>> GetByPersonAndDateRangeAsync(int personId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var workLogs = await _unitOfWork.WorkLogs.GetByPersonIdAndDateRangeAsync(personId, startDate, endDate);
                var workLogDtos = _mapper.Map<IEnumerable<WorkLogListDto>>(workLogs);
                return Result<IEnumerable<WorkLogListDto>>.Ok(workLogDtos, "Personel tarih aralığı çalışma kayıtları başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<WorkLogListDto>>.Fail($"Personel tarih aralığı çalışma kayıtları getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<WorkLogTimeSheetDto>> GetTimeSheetAsync(int personId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var person = await _unitOfWork.Persons.GetByIdAsync(personId);
                if (person == null)
                {
                    return Result<WorkLogTimeSheetDto>.Fail("Personel bulunamadı.");
                }

                var workLogs = await _unitOfWork.WorkLogs.GetByPersonIdAndDateRangeAsync(personId, startDate, endDate);
                var workLogDtos = _mapper.Map<List<WorkLogListDto>>(workLogs);

                var timeSheet = new WorkLogTimeSheetDto
                {
                    PersonId = personId,
                    PersonName = $"{person.FirstName} {person.LastName}",
                    DepartmentName = person.Department?.Name,
                    StartDate = startDate,
                    EndDate = endDate,
                    WorkLogs = workLogDtos,
                    TotalHours = workLogDtos.Sum(w => w.TotalHours),
                    TotalRegularHours = workLogDtos.Sum(w => w.RegularHours),
                    TotalOvertimeHours = workLogDtos.Sum(w => w.OvertimeHours),
                    TotalWorkDays = workLogDtos.Count,
                    LateArrivals = workLogDtos.Count(w => w.IsLateArrival),
                    EarlyDepartures = workLogDtos.Count(w => w.IsEarlyDeparture)
                };

                return Result<WorkLogTimeSheetDto>.Ok(timeSheet, "Zaman çizelgesi başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                return Result<WorkLogTimeSheetDto>.Fail($"Zaman çizelgesi oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<WorkLogTimeSheetDto>> GetWeeklyTimeSheetAsync(int personId, DateTime weekStart)
        {
            var weekEnd = weekStart.AddDays(6);
            return await GetTimeSheetAsync(personId, weekStart, weekEnd);
        }

        public async Task<Result<WorkLogTimeSheetDto>> GetMonthlyTimeSheetAsync(int personId, int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            return await GetTimeSheetAsync(personId, startDate, endDate);
        }

        public async Task<Result<IEnumerable<WorkLogListDto>>> GetOvertimeReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var workLogs = await _unitOfWork.WorkLogs.GetOvertimeWorkLogsAsync(startDate, endDate);
                var workLogDtos = _mapper.Map<IEnumerable<WorkLogListDto>>(workLogs);
                return Result<IEnumerable<WorkLogListDto>>.Ok(workLogDtos, "Mesai raporu başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<WorkLogListDto>>.Fail($"Mesai raporu getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<WorkLogListDto>>> GetLateArrivalReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var workLogs = await _unitOfWork.WorkLogs.GetLateArrivalsAsync(startDate, endDate);
                var workLogDtos = _mapper.Map<IEnumerable<WorkLogListDto>>(workLogs);
                return Result<IEnumerable<WorkLogListDto>>.Ok(workLogDtos, "Geç gelme raporu başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<WorkLogListDto>>.Fail($"Geç gelme raporu getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<WorkLogListDto>>> GetPendingApprovalsAsync()
        {
            try
            {
                var workLogs = await _unitOfWork.WorkLogs.GetPendingApprovalsAsync();
                var workLogDtos = _mapper.Map<IEnumerable<WorkLogListDto>>(workLogs);
                return Result<IEnumerable<WorkLogListDto>>.Ok(workLogDtos, "Onay bekleyen kayıtlar başarıyla getirildi.");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<WorkLogListDto>>.Fail($"Onay bekleyen kayıtlar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result> ApproveWorkLogAsync(WorkLogApprovalDto dto)
        {
            try
            {
                var workLog = await _unitOfWork.WorkLogs.GetByIdAsync(dto.Id);
                if (workLog == null)
                {
                    return Result.Fail("Çalışma kaydı bulunamadı.");
                }

                workLog.Status = "Approved";
                workLog.ApprovedById = dto.ApprovedById;
                workLog.ApprovedAt = DateTime.Now;
                workLog.ApprovalNotes = dto.ApprovalNotes;
                workLog.UpdatedAt = DateTime.Now;

                _unitOfWork.WorkLogs.Update(workLog);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok("Çalışma kaydı başarıyla onaylandı.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Çalışma kaydı onaylanırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result> RejectWorkLogAsync(WorkLogApprovalDto dto)
        {
            try
            {
                var workLog = await _unitOfWork.WorkLogs.GetByIdAsync(dto.Id);
                if (workLog == null)
                {
                    return Result.Fail("Çalışma kaydı bulunamadı.");
                }

                workLog.Status = "Rejected";
                workLog.ApprovedById = dto.ApprovedById;
                workLog.ApprovedAt = DateTime.Now;
                workLog.ApprovalNotes = dto.ApprovalNotes;
                workLog.UpdatedAt = DateTime.Now;

                _unitOfWork.WorkLogs.Update(workLog);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok("Çalışma kaydı reddedildi.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Çalışma kaydı reddedilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> GetTotalHoursByPersonAsync(int personId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var totalHours = await _unitOfWork.WorkLogs.GetTotalHoursByPersonAsync(personId, startDate, endDate);
                return Result<decimal>.Ok(totalHours, "Toplam çalışma saatleri başarıyla hesaplandı.");
            }
            catch (Exception ex)
            {
                return Result<decimal>.Fail($"Toplam çalışma saatleri hesaplanırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> GetOvertimeHoursByPersonAsync(int personId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var overtimeHours = await _unitOfWork.WorkLogs.GetOvertimeHoursByPersonAsync(personId, startDate, endDate);
                return Result<decimal>.Ok(overtimeHours, "Toplam mesai saatleri başarıyla hesaplandı.");
            }
            catch (Exception ex)
            {
                return Result<decimal>.Fail($"Toplam mesai saatleri hesaplanırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> CanCheckInAsync(int personId, DateTime date)
        {
            try
            {
                var existingWorkLog = await _unitOfWork.WorkLogs.GetByPersonIdAndDateAsync(personId, date);
                var canCheckIn = existingWorkLog == null;
                return Result<bool>.Ok(canCheckIn, canCheckIn ? "Giriş yapılabilir." : "Bu tarih için zaten giriş kaydı mevcut.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"Giriş kontrolü yapılırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> CanCheckOutAsync(int workLogId)
        {
            try
            {
                var workLog = await _unitOfWork.WorkLogs.GetByIdAsync(workLogId);
                if (workLog == null)
                {
                    return Result<bool>.Fail("Çalışma kaydı bulunamadı.");
                }

                var canCheckOut = !workLog.EndTime.HasValue;
                return Result<bool>.Ok(canCheckOut, canCheckOut ? "Çıkış yapılabilir." : "Bu kayıt için çıkış işlemi zaten yapılmış.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"Çıkış kontrolü yapılırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> HasActiveWorkLogAsync(int personId)
        {
            try
            {
                var activeWorkLogs = await _unitOfWork.WorkLogs.GetActiveWorkLogsAsync();
                var hasActive = activeWorkLogs.Any(w => w.PersonId == personId);
                return Result<bool>.Ok(hasActive, hasActive ? "Aktif çalışma kaydı mevcut." : "Aktif çalışma kaydı yok.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"Aktif çalışma kontrolü yapılırken hata oluştu: {ex.Message}");
            }
        }

        private void CalculateWorkLogTimes(WorkLog workLog)
        {
            if (workLog.EndTime.HasValue)
            {
                // Calculate total worked time
                var totalWorkedTime = workLog.EndTime.Value - workLog.StartTime;
                
                // Subtract break duration
                var breakMinutes = workLog.BreakDurationMinutes;
                var totalMinutes = totalWorkedTime.TotalMinutes - (double)breakMinutes;
                
                if (totalMinutes < 0) totalMinutes = 0;
                
                workLog.TotalHours = (decimal)(totalMinutes / 60);

                // Calculate regular and overtime hours
                if (workLog.TotalHours <= StandardWorkHours)
                {
                    workLog.RegularHours = workLog.TotalHours;
                    workLog.OvertimeHours = 0;
                    workLog.IsOvertime = false;
                }
                else
                {
                    workLog.RegularHours = StandardWorkHours;
                    workLog.OvertimeHours = workLog.TotalHours - StandardWorkHours;
                    workLog.IsOvertime = true;
                }
            }
            else
            {
                // If no end time, reset calculated fields
                workLog.TotalHours = 0;
                workLog.RegularHours = 0;
                workLog.OvertimeHours = 0;
                workLog.IsOvertime = false;
            }
        }
    }
}
