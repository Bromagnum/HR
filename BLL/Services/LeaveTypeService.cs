using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services;

public class LeaveTypeService : ILeaveTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LeaveTypeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<LeaveTypeListDto>>> GetAllAsync()
    {
        try
        {
            var leaveTypes = await _unitOfWork.LeaveTypes.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<LeaveTypeListDto>>(leaveTypes);
            return Result<IEnumerable<LeaveTypeListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveTypeListDto>>.Fail($"İzin türleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveTypeListDto>>> GetActiveAsync()
    {
        try
        {
            var leaveTypes = await _unitOfWork.LeaveTypes.GetActiveLeaveTypesAsync();
            var dtos = _mapper.Map<IEnumerable<LeaveTypeListDto>>(leaveTypes);
            return Result<IEnumerable<LeaveTypeListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveTypeListDto>>.Fail($"Aktif izin türleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveTypeDetailDto>> GetByIdAsync(int id)
    {
        try
        {
            var leaveType = await _unitOfWork.LeaveTypes.GetByIdAsync(id);
            if (leaveType == null)
            {
                return Result<LeaveTypeDetailDto>.Fail("İzin türü bulunamadı.");
            }

            var dto = _mapper.Map<LeaveTypeDetailDto>(leaveType);
            return Result<LeaveTypeDetailDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Result<LeaveTypeDetailDto>.Fail($"İzin türü getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveTypeDetailDto>> CreateAsync(LeaveTypeCreateDto dto)
    {
        try
        {
            var validationResult = await ValidateLeaveTypeAsync(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<LeaveTypeDetailDto>.Fail(validationResult.Message);
            }

            var leaveType = _mapper.Map<LeaveType>(dto);
            leaveType.CreatedAt = DateTime.Now;
            leaveType.IsActive = true;

            await _unitOfWork.LeaveTypes.AddAsync(leaveType);
            await _unitOfWork.SaveChangesAsync();

            var detailDto = _mapper.Map<LeaveTypeDetailDto>(leaveType);
            return Result<LeaveTypeDetailDto>.Ok(detailDto);
        }
        catch (Exception ex)
        {
            return Result<LeaveTypeDetailDto>.Fail($"İzin türü oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveTypeDetailDto>> UpdateAsync(LeaveTypeUpdateDto dto)
    {
        try
        {
            var validationResult = await ValidateLeaveTypeAsync(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<LeaveTypeDetailDto>.Fail(validationResult.Message);
            }

            var leaveType = await _unitOfWork.LeaveTypes.GetByIdAsync(dto.Id);
            if (leaveType == null)
            {
                return Result<LeaveTypeDetailDto>.Fail("İzin türü bulunamadı.");
            }

            _mapper.Map(dto, leaveType);
            leaveType.UpdatedAt = DateTime.Now;

            _unitOfWork.LeaveTypes.Update(leaveType);
            await _unitOfWork.SaveChangesAsync();

            var detailDto = _mapper.Map<LeaveTypeDetailDto>(leaveType);
            return Result<LeaveTypeDetailDto>.Ok(detailDto);
        }
        catch (Exception ex)
        {
            return Result<LeaveTypeDetailDto>.Fail($"İzin türü güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var canDeleteResult = await CanDeleteLeaveTypeAsync(id);
            if (!canDeleteResult.IsSuccess)
            {
                return Result<bool>.Fail(canDeleteResult.Message);
            }

            var leaveType = await _unitOfWork.LeaveTypes.GetByIdAsync(id);
            if (leaveType == null)
            {
                return Result<bool>.Fail("İzin türü bulunamadı.");
            }

            _unitOfWork.LeaveTypes.Remove(leaveType);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"İzin türü silinirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ToggleStatusAsync(int id)
    {
        try
        {
            var leaveType = await _unitOfWork.LeaveTypes.GetByIdAsync(id);
            if (leaveType == null)
            {
                return Result<bool>.Fail("İzin türü bulunamadı.");
            }

            leaveType.IsActive = !leaveType.IsActive;
            leaveType.UpdatedAt = DateTime.Now;

            _unitOfWork.LeaveTypes.Update(leaveType);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"İzin türü durumu güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<LeaveTypeDetailDto>> GetByNameAsync(string name)
    {
        try
        {
            var leaveType = await _unitOfWork.LeaveTypes.GetByNameAsync(name);
            if (leaveType == null)
            {
                return Result<LeaveTypeDetailDto>.Fail("İzin türü bulunamadı.");
            }

            var dto = _mapper.Map<LeaveTypeDetailDto>(leaveType);
            return Result<LeaveTypeDetailDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Result<LeaveTypeDetailDto>.Fail($"İzin türü getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveTypeListDto>>> GetLeaveTypesWithBalancesAsync(int personId, int year)
    {
        try
        {
            var leaveTypes = await _unitOfWork.LeaveTypes.GetLeaveTypesWithBalancesAsync(personId, year);
            var dtos = _mapper.Map<IEnumerable<LeaveTypeListDto>>(leaveTypes);
            return Result<IEnumerable<LeaveTypeListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveTypeListDto>>.Fail($"İzin türleri getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ValidateLeaveTypeAsync(LeaveTypeCreateDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Result<bool>.Fail("İzin türü adı boş olamaz.");
            }

            if (dto.Name.Length > 100)
            {
                return Result<bool>.Fail("İzin türü adı 100 karakterden uzun olamaz.");
            }

            var existingLeaveType = await _unitOfWork.LeaveTypes.GetByNameAsync(dto.Name);
            if (existingLeaveType != null)
            {
                return Result<bool>.Fail("Bu isimde bir izin türü zaten mevcut.");
            }

            if (dto.MaxDaysPerYear < 0)
            {
                return Result<bool>.Fail("Maksimum gün sayısı negatif olamaz.");
            }

            if (dto.MaxCarryOverDays < 0)
            {
                return Result<bool>.Fail("Maksimum devir gün sayısı negatif olamaz.");
            }

            if (dto.CanCarryOver && dto.MaxCarryOverDays == 0)
            {
                return Result<bool>.Fail("Devir izni varsa maksimum devir gün sayısı 0'dan büyük olmalıdır.");
            }

            if (dto.NotificationDays < 0)
            {
                return Result<bool>.Fail("Bildirim gün sayısı negatif olamaz.");
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Doğrulama sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ValidateLeaveTypeAsync(LeaveTypeUpdateDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Result<bool>.Fail("İzin türü adı boş olamaz.");
            }

            if (dto.Name.Length > 100)
            {
                return Result<bool>.Fail("İzin türü adı 100 karakterden uzun olamaz.");
            }

            var existingLeaveType = await _unitOfWork.LeaveTypes.GetByNameAsync(dto.Name);
            if (existingLeaveType != null && existingLeaveType.Id != dto.Id)
            {
                return Result<bool>.Fail("Bu isimde bir izin türü zaten mevcut.");
            }

            if (dto.MaxDaysPerYear < 0)
            {
                return Result<bool>.Fail("Maksimum gün sayısı negatif olamaz.");
            }

            if (dto.MaxCarryOverDays < 0)
            {
                return Result<bool>.Fail("Maksimum devir gün sayısı negatif olamaz.");
            }

            if (dto.CanCarryOver && dto.MaxCarryOverDays == 0)
            {
                return Result<bool>.Fail("Devir izni varsa maksimum devir gün sayısı 0'dan büyük olmalıdır.");
            }

            if (dto.NotificationDays < 0)
            {
                return Result<bool>.Fail("Bildirim gün sayısı negatif olamaz.");
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Doğrulama sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> CanDeleteLeaveTypeAsync(int id)
    {
        try
        {
            var leaveType = await _unitOfWork.LeaveTypes.GetByIdAsync(id);
            if (leaveType == null)
            {
                return Result<bool>.Fail("İzin türü bulunamadı.");
            }

            if (leaveType.Leaves.Any())
            {
                return Result<bool>.Fail("Bu izin türüne ait izin kayıtları olduğu için silinemez.");
            }

            if (leaveType.LeaveBalances.Any())
            {
                return Result<bool>.Fail("Bu izin türüne ait bakiye kayıtları olduğu için silinemez.");
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Silme kontrolü sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<Dictionary<string, object>>> GetLeaveTypeStatisticsAsync(int leaveTypeId, int year)
    {
        try
        {
            var leaveType = await _unitOfWork.LeaveTypes.GetByIdAsync(leaveTypeId);
            if (leaveType == null)
            {
                return Result<Dictionary<string, object>>.Fail("İzin türü bulunamadı.");
            }

            var leaves = leaveType.Leaves.Where(l => l.StartDate.Year == year).ToList();
            var balances = leaveType.LeaveBalances.Where(lb => lb.Year == year).ToList();

            var statistics = new Dictionary<string, object>
            {
                ["LeaveTypeName"] = leaveType.Name,
                ["Year"] = year,
                ["TotalLeaves"] = leaves.Count,
                ["ApprovedLeaves"] = leaves.Count(l => l.Status == LeaveStatus.Approved),
                ["PendingLeaves"] = leaves.Count(l => l.Status == LeaveStatus.Pending),
                ["RejectedLeaves"] = leaves.Count(l => l.Status == LeaveStatus.Rejected),
                ["TotalDaysRequested"] = leaves.Sum(l => l.TotalDays),
                ["TotalDaysApproved"] = leaves.Where(l => l.Status == LeaveStatus.Approved).Sum(l => l.TotalDays),
                ["AverageLeaveLength"] = leaves.Any() ? leaves.Average(l => l.TotalDays) : 0,
                ["TotalEmployeesWithBalance"] = balances.Count,
                ["TotalAllocatedDays"] = balances.Sum(b => b.AllocatedDays),
                ["TotalUsedDays"] = balances.Sum(b => b.UsedDays),
                ["TotalAvailableDays"] = balances.Sum(b => b.AvailableDays),
                ["UsagePercentage"] = balances.Sum(b => b.AllocatedDays) > 0 ? 
                    (balances.Sum(b => b.UsedDays) / balances.Sum(b => b.AllocatedDays)) * 100 : 0
            };

            return Result<Dictionary<string, object>>.Ok(statistics);
        }
        catch (Exception ex)
        {
            return Result<Dictionary<string, object>>.Fail($"İstatistikler hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<Dictionary<string, object>>> GetAllLeaveTypesStatisticsAsync(int year)
    {
        try
        {
            var leaveTypes = await _unitOfWork.LeaveTypes.GetActiveLeaveTypesAsync();
            var statistics = new Dictionary<string, object>();

            foreach (var leaveType in leaveTypes)
            {
                var leaveTypeStats = await GetLeaveTypeStatisticsAsync(leaveType.Id, year);
                if (leaveTypeStats.IsSuccess)
                {
                    if (leaveTypeStats.Data != null)
                        statistics[leaveType.Name] = leaveTypeStats.Data;
                }
            }

            return Result<Dictionary<string, object>>.Ok(statistics);
        }
        catch (Exception ex)
        {
            return Result<Dictionary<string, object>>.Fail($"İstatistikler hesaplanırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<LeaveTypeListDto>>> GetMostUsedLeaveTypesAsync(int year, int count = 5)
    {
        try
        {
            var leaveTypes = await _unitOfWork.LeaveTypes.GetActiveLeaveTypesAsync();
            var leaveTypesWithUsage = new List<(LeaveType LeaveType, int Usage)>();

            foreach (var leaveType in leaveTypes)
            {
                var usage = leaveType.Leaves.Where(l => l.StartDate.Year == year && 
                                                       l.Status == LeaveStatus.Approved).Sum(l => l.TotalDays);
                leaveTypesWithUsage.Add((leaveType, usage));
            }

            var mostUsed = leaveTypesWithUsage
                .OrderByDescending(x => x.Usage)
                .Take(count)
                .Select(x => x.LeaveType);

            var dtos = _mapper.Map<IEnumerable<LeaveTypeListDto>>(mostUsed);
            return Result<IEnumerable<LeaveTypeListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<LeaveTypeListDto>>.Fail($"En çok kullanılan izin türleri getirilirken hata oluştu: {ex.Message}");
        }
    }
}
