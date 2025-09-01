using AutoMapper;
using BLL.Utilities;
using BLL.DTOs;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services;

public class QualificationService : IQualificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QualificationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<QualificationListDto>>> GetAllAsync()
    {
        try
        {
            var qualifications = await _unitOfWork.Qualifications.GetAllAsync();
            var qualificationDtos = _mapper.Map<IEnumerable<QualificationListDto>>(qualifications);
            
            return Result<IEnumerable<QualificationListDto>>.SuccessResult(qualificationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<QualificationListDto>>.ErrorResult($"Yeterlilik bilgileri alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<QualificationDetailDto>> GetByIdAsync(int id)
    {
        try
        {
            var qualification = await _unitOfWork.Qualifications.GetQualificationWithPersonAsync(id);
            if (qualification == null)
                return Result<QualificationDetailDto>.ErrorResult("Yeterlilik bilgisi bulunamadı.");

            var qualificationDto = _mapper.Map<QualificationDetailDto>(qualification);
            return Result<QualificationDetailDto>.SuccessResult(qualificationDto);
        }
        catch (Exception ex)
        {
            return Result<QualificationDetailDto>.ErrorResult($"Yeterlilik bilgisi alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<QualificationDetailDto>> CreateAsync(QualificationCreateDto qualificationCreateDto)
    {
        try
        {
            // Validate person exists
            var person = await _unitOfWork.Persons.GetByIdAsync(qualificationCreateDto.PersonId);
            if (person == null)
                return Result<QualificationDetailDto>.ErrorResult("Seçilen personel bulunamadı.");

            // Check expiration date logic
            if (qualificationCreateDto.HasExpiration && !qualificationCreateDto.ExpirationDate.HasValue)
                return Result<QualificationDetailDto>.ErrorResult("Süre sınırı varsa son geçerlilik tarihi girilmelidir.");

            if (qualificationCreateDto.ExpirationDate.HasValue && qualificationCreateDto.ExpirationDate.Value <= qualificationCreateDto.IssueDate)
                return Result<QualificationDetailDto>.ErrorResult("Son geçerlilik tarihi, veriliş tarihinden sonra olmalıdır.");

            var qualification = _mapper.Map<Qualification>(qualificationCreateDto);
            qualification.CreatedAt = DateTime.Now;
            qualification.IsActive = true;

            await _unitOfWork.Qualifications.AddAsync(qualification);
            await _unitOfWork.SaveChangesAsync();

            var createdQualification = await _unitOfWork.Qualifications.GetQualificationWithPersonAsync(qualification.Id);
            if (createdQualification == null)
                return Result<QualificationDetailDto>.ErrorResult("Yeterlilik bilgisi kaydedildi ancak tekrar alınamadı.");

            var qualificationDto = _mapper.Map<QualificationDetailDto>(createdQualification);

            return Result<QualificationDetailDto>.SuccessResult(qualificationDto, "Yeterlilik bilgisi başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            return Result<QualificationDetailDto>.ErrorResult($"Yeterlilik bilgisi eklenirken hata oluştu: {ex.Message}. Inner: {ex.InnerException?.Message ?? "N/A"}");
        }
    }

    public async Task<Result<QualificationDetailDto>> UpdateAsync(QualificationUpdateDto qualificationUpdateDto)
    {
        try
        {
            var qualification = await _unitOfWork.Qualifications.GetByIdAsync(qualificationUpdateDto.Id);
            if (qualification == null)
                return Result<QualificationDetailDto>.ErrorResult("Yeterlilik bilgisi bulunamadı.");

            // Validate person exists
            var person = await _unitOfWork.Persons.GetByIdAsync(qualificationUpdateDto.PersonId);
            if (person == null)
                return Result<QualificationDetailDto>.ErrorResult("Seçilen personel bulunamadı.");

            // Check expiration date logic
            if (qualificationUpdateDto.HasExpiration && !qualificationUpdateDto.ExpirationDate.HasValue)
                return Result<QualificationDetailDto>.ErrorResult("Süre sınırı varsa son geçerlilik tarihi girilmelidir.");

            if (qualificationUpdateDto.ExpirationDate.HasValue && qualificationUpdateDto.ExpirationDate.Value <= qualificationUpdateDto.IssueDate)
                return Result<QualificationDetailDto>.ErrorResult("Son geçerlilik tarihi, veriliş tarihinden sonra olmalıdır.");

            _mapper.Map(qualificationUpdateDto, qualification);
            qualification.UpdatedAt = DateTime.Now;

            _unitOfWork.Qualifications.Update(qualification);
            await _unitOfWork.SaveChangesAsync();

            var updatedQualification = await _unitOfWork.Qualifications.GetQualificationWithPersonAsync(qualification.Id);
            var qualificationDto = _mapper.Map<QualificationDetailDto>(updatedQualification);

            return Result<QualificationDetailDto>.SuccessResult(qualificationDto, "Yeterlilik bilgisi başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            return Result<QualificationDetailDto>.ErrorResult($"Yeterlilik bilgisi güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            var qualification = await _unitOfWork.Qualifications.GetByIdAsync(id);
            if (qualification == null)
                return Result.ErrorResult("Yeterlilik bilgisi bulunamadı.");

            _unitOfWork.Qualifications.Remove(qualification);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult("Yeterlilik bilgisi başarıyla silindi.");
        }
        catch (Exception ex)
        {
            return Result.ErrorResult($"Yeterlilik bilgisi silinirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result> ChangeStatusAsync(int id, bool isActive)
    {
        try
        {
            var qualification = await _unitOfWork.Qualifications.GetByIdAsync(id);
            if (qualification == null)
                return Result.ErrorResult("Yeterlilik bilgisi bulunamadı.");

            qualification.IsActive = isActive;
            qualification.UpdatedAt = DateTime.Now;

            _unitOfWork.Qualifications.Update(qualification);
            await _unitOfWork.SaveChangesAsync();

            var statusText = isActive ? "aktif" : "pasif";
            return Result.SuccessResult($"Yeterlilik bilgisi durumu {statusText} olarak güncellendi.");
        }
        catch (Exception ex)
        {
            return Result.ErrorResult($"Yeterlilik bilgisi durumu güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<QualificationListDto>>> GetByPersonIdAsync(int personId)
    {
        try
        {
            var qualifications = await _unitOfWork.Qualifications.GetByPersonIdAsync(personId);
            var qualificationDtos = _mapper.Map<IEnumerable<QualificationListDto>>(qualifications);
            
            return Result<IEnumerable<QualificationListDto>>.SuccessResult(qualificationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<QualificationListDto>>.ErrorResult($"Personel yeterlilikleri alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<QualificationListDto>>> GetExpiringSoonAsync(int days = 30)
    {
        try
        {
            var qualifications = await _unitOfWork.Qualifications.GetExpiringSoonAsync(days);
            var qualificationDtos = _mapper.Map<IEnumerable<QualificationListDto>>(qualifications);
            
            return Result<IEnumerable<QualificationListDto>>.SuccessResult(qualificationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<QualificationListDto>>.ErrorResult($"Süresi yaklaşan yeterlilikler alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<QualificationListDto>>> GetExpiredAsync()
    {
        try
        {
            var qualifications = await _unitOfWork.Qualifications.GetExpiredAsync();
            var qualificationDtos = _mapper.Map<IEnumerable<QualificationListDto>>(qualifications);
            
            return Result<IEnumerable<QualificationListDto>>.SuccessResult(qualificationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<QualificationListDto>>.ErrorResult($"Süresi dolmuş yeterlilikler alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<QualificationListDto>>> GetByCategoryAsync(string category)
    {
        try
        {
            var qualifications = await _unitOfWork.Qualifications.GetByCategoryAsync(category);
            var qualificationDtos = _mapper.Map<IEnumerable<QualificationListDto>>(qualifications);
            
            return Result<IEnumerable<QualificationListDto>>.SuccessResult(qualificationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<QualificationListDto>>.ErrorResult($"Kategori yeterlilikleri alınırken hata oluştu: {ex.Message}");
        }
    }
}
