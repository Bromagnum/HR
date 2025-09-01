using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services;

public class PersonService : IPersonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PersonService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<PersonListDto>>> GetAllAsync()
    {
        try
        {
            var persons = await _unitOfWork.Persons.GetAllAsync();
            var personDtos = _mapper.Map<IEnumerable<PersonListDto>>(persons);
            
            return Result<IEnumerable<PersonListDto>>.SuccessResult(personDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PersonListDto>>.ErrorResult($"Personel listesi alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<PersonDetailDto>> GetByIdAsync(int id)
    {
        try
        {
            var person = await _unitOfWork.Persons.GetByIdAsync(id);
            if (person == null)
            {
                return Result<PersonDetailDto>.ErrorResult("Personel bulunamadı.");
            }

            var personDto = _mapper.Map<PersonDetailDto>(person);
            return Result<PersonDetailDto>.SuccessResult(personDto);
        }
        catch (Exception ex)
        {
            return Result<PersonDetailDto>.ErrorResult($"Personel getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<PersonDetailDto>> GetByTcKimlikNoAsync(string tcKimlikNo)
    {
        try
        {
            var person = await _unitOfWork.Persons.GetByTcKimlikNoAsync(tcKimlikNo);
            if (person == null)
            {
                return Result<PersonDetailDto>.ErrorResult("Personel bulunamadı.");
            }

            var personDto = _mapper.Map<PersonDetailDto>(person);
            return Result<PersonDetailDto>.SuccessResult(personDto);
        }
        catch (Exception ex)
        {
            return Result<PersonDetailDto>.ErrorResult($"Personel getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PersonListDto>>> GetByDepartmentIdAsync(int departmentId)
    {
        try
        {
            var persons = await _unitOfWork.Persons.GetByDepartmentIdAsync(departmentId);
            var personDtos = _mapper.Map<IEnumerable<PersonListDto>>(persons);
            
            return Result<IEnumerable<PersonListDto>>.SuccessResult(personDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PersonListDto>>.ErrorResult($"Departman personelleri alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PersonListDto>>> GetActiveEmployeesAsync()
    {
        try
        {
            var persons = await _unitOfWork.Persons.GetActiveEmployeesAsync();
            var personDtos = _mapper.Map<IEnumerable<PersonListDto>>(persons);
            
            return Result<IEnumerable<PersonListDto>>.SuccessResult(personDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PersonListDto>>.ErrorResult($"Aktif personeller alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<PersonDetailDto>> CreateAsync(PersonCreateDto dto)
    {
        try
        {
            // TC Kimlik No kontrolü
            var existingPerson = await _unitOfWork.Persons.GetByTcKimlikNoAsync(dto.TcKimlikNo);
            if (existingPerson != null)
            {
                return Result<PersonDetailDto>.ErrorResult("Bu TC Kimlik No ile kayıtlı personel zaten mevcut.");
            }

            // Departman kontrolü
            if (dto.DepartmentId.HasValue)
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(dto.DepartmentId.Value);
                if (department == null)
                {
                    return Result<PersonDetailDto>.ErrorResult("Geçersiz departman seçimi.");
                }
            }

            var person = _mapper.Map<Person>(dto);
            await _unitOfWork.Persons.AddAsync(person);
            await _unitOfWork.SaveChangesAsync();

            var createdPerson = await _unitOfWork.Persons.GetByIdAsync(person.Id);
            var personDto = _mapper.Map<PersonDetailDto>(createdPerson);

            return Result<PersonDetailDto>.SuccessResult(personDto, "Personel başarıyla oluşturuldu.");
        }
        catch (Exception ex)
        {
            return Result<PersonDetailDto>.ErrorResult($"Personel oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<PersonDetailDto>> UpdateAsync(PersonUpdateDto dto)
    {
        try
        {
            var person = await _unitOfWork.Persons.GetByIdAsync(dto.Id);
            if (person == null)
            {
                return Result<PersonDetailDto>.ErrorResult("Personel bulunamadı.");
            }

            // TC Kimlik No kontrolü (kendisi hariç)
            var existingPerson = await _unitOfWork.Persons.GetByTcKimlikNoAsync(dto.TcKimlikNo);
            if (existingPerson != null && existingPerson.Id != dto.Id)
            {
                return Result<PersonDetailDto>.ErrorResult("Bu TC Kimlik No ile kayıtlı başka bir personel mevcut.");
            }

            // Departman kontrolü
            if (dto.DepartmentId.HasValue)
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(dto.DepartmentId.Value);
                if (department == null)
                {
                    return Result<PersonDetailDto>.ErrorResult("Geçersiz departman seçimi.");
                }
            }

            _mapper.Map(dto, person);
            _unitOfWork.Persons.Update(person);
            await _unitOfWork.SaveChangesAsync();

            var updatedPerson = await _unitOfWork.Persons.GetByIdAsync(person.Id);
            var personDto = _mapper.Map<PersonDetailDto>(updatedPerson);

            return Result<PersonDetailDto>.SuccessResult(personDto, "Personel başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            return Result<PersonDetailDto>.ErrorResult($"Personel güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            var person = await _unitOfWork.Persons.GetByIdAsync(id);
            if (person == null)
            {
                return Result.ErrorResult("Personel bulunamadı.");
            }

            _unitOfWork.Persons.Remove(person);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult("Personel başarıyla silindi.");
        }
        catch (Exception ex)
        {
            return Result.ErrorResult($"Personel silinirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result> SetActiveStatusAsync(int id, bool isActive)
    {
        try
        {
            var person = await _unitOfWork.Persons.GetByIdAsync(id);
            if (person == null)
            {
                return Result.ErrorResult("Personel bulunamadı.");
            }

            person.IsActive = isActive;
            _unitOfWork.Persons.Update(person);
            await _unitOfWork.SaveChangesAsync();

            var statusText = isActive ? "aktif" : "pasif";
            return Result.SuccessResult($"Personel durumu {statusText} olarak güncellendi.");
        }
        catch (Exception ex)
        {
            return Result.ErrorResult($"Personel durumu güncellenirken hata oluştu: {ex.Message}");
        }
    }
}
