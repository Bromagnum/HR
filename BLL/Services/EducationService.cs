using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services;

public class EducationService : IEducationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EducationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<EducationListDto>>> GetAllAsync()
    {
        try
        {
            var educations = await _unitOfWork.Educations.GetAllAsync();
            var educationDtos = _mapper.Map<IEnumerable<EducationListDto>>(educations);
            return Result<IEnumerable<EducationListDto>>.SuccessResult(educationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EducationListDto>>.ErrorResult($"Eğitim bilgileri alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<EducationDetailDto?>> GetByIdAsync(int id)
    {
        try
        {
            var education = await _unitOfWork.Educations.GetEducationWithPersonAsync(id);
            if (education == null)
                return Result<EducationDetailDto?>.ErrorResult("Eğitim bilgisi bulunamadı.");

            var educationDto = _mapper.Map<EducationDetailDto>(education);
            return Result<EducationDetailDto?>.SuccessResult(educationDto);
        }
        catch (Exception ex)
        {
            return Result<EducationDetailDto?>.ErrorResult($"Eğitim bilgisi alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<EducationListDto>>> GetByPersonIdAsync(int personId)
    {
        try
        {
            var educations = await _unitOfWork.Educations.GetByPersonIdAsync(personId);
            var educationDtos = _mapper.Map<IEnumerable<EducationListDto>>(educations);
            return Result<IEnumerable<EducationListDto>>.SuccessResult(educationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EducationListDto>>.ErrorResult($"Personel eğitim bilgileri alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<EducationListDto>>> GetOngoingEducationsAsync()
    {
        try
        {
            var educations = await _unitOfWork.Educations.GetOngoingEducationsAsync();
            var educationDtos = _mapper.Map<IEnumerable<EducationListDto>>(educations);
            return Result<IEnumerable<EducationListDto>>.SuccessResult(educationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EducationListDto>>.ErrorResult($"Devam eden eğitimler alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<EducationListDto>>> GetCompletedEducationsAsync()
    {
        try
        {
            var educations = await _unitOfWork.Educations.GetCompletedEducationsAsync();
            var educationDtos = _mapper.Map<IEnumerable<EducationListDto>>(educations);
            return Result<IEnumerable<EducationListDto>>.SuccessResult(educationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EducationListDto>>.ErrorResult($"Tamamlanan eğitimler alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<EducationListDto>>> GetEducationsByDegreeAsync(string degree)
    {
        try
        {
            var educations = await _unitOfWork.Educations.GetEducationsByDegreeAsync(degree);
            var educationDtos = _mapper.Map<IEnumerable<EducationListDto>>(educations);
            return Result<IEnumerable<EducationListDto>>.SuccessResult(educationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EducationListDto>>.ErrorResult($"Dereceye göre eğitimler alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<EducationDetailDto>> CreateAsync(EducationCreateDto educationCreateDto)
    {
        try
        {
            // Validate person exists
            var person = await _unitOfWork.Persons.GetByIdAsync(educationCreateDto.PersonId);
            if (person == null)
                return Result<EducationDetailDto>.ErrorResult("Seçilen personel bulunamadı.");

            // Validate dates
            if (educationCreateDto.EndDate.HasValue && educationCreateDto.StartDate > educationCreateDto.EndDate)
                return Result<EducationDetailDto>.ErrorResult("Başlangıç tarihi bitiş tarihinden önce olmalıdır.");

            if (!educationCreateDto.IsOngoing && !educationCreateDto.EndDate.HasValue)
                return Result<EducationDetailDto>.ErrorResult("Devam etmeyen eğitimler için bitiş tarihi gereklidir.");

            var education = _mapper.Map<Education>(educationCreateDto);
            education.CreatedAt = DateTime.Now;
            education.IsActive = true;

            await _unitOfWork.Educations.AddAsync(education);
            await _unitOfWork.SaveChangesAsync();

            var createdEducation = await _unitOfWork.Educations.GetEducationWithPersonAsync(education.Id);
            if (createdEducation == null)
                return Result<EducationDetailDto>.ErrorResult("Eğitim bilgisi kaydedildi ancak tekrar alınamadı.");

            var educationDto = _mapper.Map<EducationDetailDto>(createdEducation);

            return Result<EducationDetailDto>.SuccessResult(educationDto, "Eğitim bilgisi başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            return Result<EducationDetailDto>.ErrorResult($"Eğitim bilgisi eklenirken hata oluştu: {ex.Message}. Inner: {ex.InnerException?.Message ?? "N/A"}");
        }
    }

    public async Task<Result<EducationDetailDto>> UpdateAsync(EducationUpdateDto educationUpdateDto)
    {
        try
        {
            var existingEducation = await _unitOfWork.Educations.GetByIdAsync(educationUpdateDto.Id);
            if (existingEducation == null)
                return Result<EducationDetailDto>.ErrorResult("Güncellenecek eğitim bilgisi bulunamadı.");

            // Validate person exists
            var person = await _unitOfWork.Persons.GetByIdAsync(educationUpdateDto.PersonId);
            if (person == null)
                return Result<EducationDetailDto>.ErrorResult("Seçilen personel bulunamadı.");

            // Validate dates
            if (educationUpdateDto.EndDate.HasValue && educationUpdateDto.StartDate > educationUpdateDto.EndDate)
                return Result<EducationDetailDto>.ErrorResult("Başlangıç tarihi bitiş tarihinden önce olmalıdır.");

            if (!educationUpdateDto.IsOngoing && !educationUpdateDto.EndDate.HasValue)
                return Result<EducationDetailDto>.ErrorResult("Devam etmeyen eğitimler için bitiş tarihi gereklidir.");

            _mapper.Map(educationUpdateDto, existingEducation);
            existingEducation.UpdatedAt = DateTime.Now;

            _unitOfWork.Educations.Update(existingEducation);
            await _unitOfWork.SaveChangesAsync();

            var updatedEducation = await _unitOfWork.Educations.GetEducationWithPersonAsync(existingEducation.Id);
            var educationDto = _mapper.Map<EducationDetailDto>(updatedEducation);

            return Result<EducationDetailDto>.SuccessResult(educationDto, "Eğitim bilgisi başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            return Result<EducationDetailDto>.ErrorResult($"Eğitim bilgisi güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            var education = await _unitOfWork.Educations.GetByIdAsync(id);
            if (education == null)
                return Result.ErrorResult("Silinecek eğitim bilgisi bulunamadı.");

            education.IsActive = false;
            education.UpdatedAt = DateTime.Now;

            _unitOfWork.Educations.Update(education);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult("Eğitim bilgisi başarıyla silindi.");
        }
        catch (Exception ex)
        {
            return Result.ErrorResult($"Eğitim bilgisi silinirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result> ChangeStatusAsync(int id, bool isActive)
    {
        try
        {
            var education = await _unitOfWork.Educations.GetByIdAsync(id);
            if (education == null)
                return Result.ErrorResult("Eğitim bilgisi bulunamadı.");

            education.IsActive = isActive;
            education.UpdatedAt = DateTime.Now;

            _unitOfWork.Educations.Update(education);
            await _unitOfWork.SaveChangesAsync();

            var statusText = isActive ? "aktif" : "pasif";
            return Result.SuccessResult($"Eğitim bilgisi durumu {statusText} olarak güncellendi.");
        }
        catch (Exception ex)
        {
            return Result.ErrorResult($"Eğitim bilgisi durumu güncellenirken hata oluştu: {ex.Message}");
        }
    }
}
