using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<DepartmentListDto>>> GetAllAsync()
    {
        try
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            var departmentDtos = _mapper.Map<IEnumerable<DepartmentListDto>>(departments);
            
            return Result<IEnumerable<DepartmentListDto>>.SuccessResult(departmentDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<DepartmentListDto>>.ErrorResult($"Departman listesi alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<DepartmentDetailDto>> GetByIdAsync(int id)
    {
        try
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                return Result<DepartmentDetailDto>.ErrorResult("Departman bulunamadı.");
            }

            var departmentDto = _mapper.Map<DepartmentDetailDto>(department);
            return Result<DepartmentDetailDto>.SuccessResult(departmentDto);
        }
        catch (Exception ex)
        {
            return Result<DepartmentDetailDto>.ErrorResult($"Departman getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<DepartmentListDto>>> GetRootDepartmentsAsync()
    {
        try
        {
            var departments = await _unitOfWork.Departments.GetRootDepartmentsAsync();
            var departmentDtos = _mapper.Map<IEnumerable<DepartmentListDto>>(departments);
            
            return Result<IEnumerable<DepartmentListDto>>.SuccessResult(departmentDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<DepartmentListDto>>.ErrorResult($"Ana departmanlar alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<DepartmentListDto>>> GetSubDepartmentsAsync(int parentId)
    {
        try
        {
            var departments = await _unitOfWork.Departments.GetSubDepartmentsAsync(parentId);
            var departmentDtos = _mapper.Map<IEnumerable<DepartmentListDto>>(departments);
            
            return Result<IEnumerable<DepartmentListDto>>.SuccessResult(departmentDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<DepartmentListDto>>.ErrorResult($"Alt departmanlar alınırken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<DepartmentDetailDto>> CreateAsync(DepartmentCreateDto dto)
    {
        try
        {
            // Üst departman kontrolü
            if (dto.ParentDepartmentId.HasValue)
            {
                var parentDepartment = await _unitOfWork.Departments.GetByIdAsync(dto.ParentDepartmentId.Value);
                if (parentDepartment == null)
                {
                    return Result<DepartmentDetailDto>.ErrorResult("Geçersiz üst departman seçimi.");
                }
            }

            var department = _mapper.Map<Department>(dto);
            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            var createdDepartment = await _unitOfWork.Departments.GetByIdAsync(department.Id);
            var departmentDto = _mapper.Map<DepartmentDetailDto>(createdDepartment);

            return Result<DepartmentDetailDto>.SuccessResult(departmentDto, "Departman başarıyla oluşturuldu.");
        }
        catch (Exception ex)
        {
            return Result<DepartmentDetailDto>.ErrorResult($"Departman oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<DepartmentDetailDto>> UpdateAsync(DepartmentUpdateDto dto)
    {
        try
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(dto.Id);
            if (department == null)
            {
                return Result<DepartmentDetailDto>.ErrorResult("Departman bulunamadı.");
            }

            // Üst departman kontrolü
            if (dto.ParentDepartmentId.HasValue)
            {
                if (dto.ParentDepartmentId == dto.Id)
                {
                    return Result<DepartmentDetailDto>.ErrorResult("Departman kendisinin alt departmanı olamaz.");
                }

                var parentDepartment = await _unitOfWork.Departments.GetByIdAsync(dto.ParentDepartmentId.Value);
                if (parentDepartment == null)
                {
                    return Result<DepartmentDetailDto>.ErrorResult("Geçersiz üst departman seçimi.");
                }
            }

            _mapper.Map(dto, department);
            _unitOfWork.Departments.Update(department);
            await _unitOfWork.SaveChangesAsync();

            var updatedDepartment = await _unitOfWork.Departments.GetByIdAsync(department.Id);
            var departmentDto = _mapper.Map<DepartmentDetailDto>(updatedDepartment);

            return Result<DepartmentDetailDto>.SuccessResult(departmentDto, "Departman başarıyla güncellendi.");
        }
        catch (Exception ex)
        {
            return Result<DepartmentDetailDto>.ErrorResult($"Departman güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                return Result.ErrorResult("Departman bulunamadı.");
            }

            // Alt departman kontrolü
            if (department.SubDepartments.Any(d => d.IsActive))
            {
                return Result.ErrorResult("Alt departmanları olan departman silinemez.");
            }

            // Personel kontrolü
            if (department.Employees.Any(e => e.IsActive))
            {
                return Result.ErrorResult("Personeli olan departman silinemez.");
            }

            _unitOfWork.Departments.Remove(department);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult("Departman başarıyla silindi.");
        }
        catch (Exception ex)
        {
            return Result.ErrorResult($"Departman silinirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result> SetActiveStatusAsync(int id, bool isActive)
    {
        try
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
            {
                return Result.ErrorResult("Departman bulunamadı.");
            }

            department.IsActive = isActive;
            _unitOfWork.Departments.Update(department);
            await _unitOfWork.SaveChangesAsync();

            var statusText = isActive ? "aktif" : "pasif";
            return Result.SuccessResult($"Departman durumu {statusText} olarak güncellendi.");
        }
        catch (Exception ex)
        {
            return Result.ErrorResult($"Departman durumu güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<DepartmentSearchResultDto>> SearchAsync(DepartmentFilterDto filter)
    {
        try
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            
            // Filtreleme
            var filteredDepartments = departments.AsEnumerable();
            
            // Arama terimi filtrelemesi
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLowerInvariant();
                filteredDepartments = filteredDepartments.Where(d => 
                    d.Name.ToLowerInvariant().Contains(searchTerm) ||
                    (!string.IsNullOrEmpty(d.Description) && d.Description.ToLowerInvariant().Contains(searchTerm))
                );
            }
            
            // Aktif/Pasif filtrelemesi
            if (filter.IsActive.HasValue)
            {
                filteredDepartments = filteredDepartments.Where(d => d.IsActive == filter.IsActive.Value);
            }
            
            // Üst departman filtrelemesi
            if (filter.HasParent.HasValue)
            {
                if (filter.HasParent.Value)
                {
                    filteredDepartments = filteredDepartments.Where(d => d.ParentDepartmentId != null);
                }
                else
                {
                    filteredDepartments = filteredDepartments.Where(d => d.ParentDepartmentId == null);
                }
            }
            
            // Belirli parent departman filtrelemesi
            if (filter.ParentDepartmentId.HasValue)
            {
                filteredDepartments = filteredDepartments.Where(d => d.ParentDepartmentId == filter.ParentDepartmentId.Value);
            }
            
            // Sıralama
            filteredDepartments = filter.SortBy?.ToLowerInvariant() switch
            {
                "name" => filter.SortDescending 
                    ? filteredDepartments.OrderByDescending(d => d.Name)
                    : filteredDepartments.OrderBy(d => d.Name),
                "description" => filter.SortDescending
                    ? filteredDepartments.OrderByDescending(d => d.Description)
                    : filteredDepartments.OrderBy(d => d.Description),
                "createdat" => filter.SortDescending
                    ? filteredDepartments.OrderByDescending(d => d.CreatedAt)
                    : filteredDepartments.OrderBy(d => d.CreatedAt),
                _ => filteredDepartments.OrderBy(d => d.Name)
            };
            
            var totalCount = filteredDepartments.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);
            
            // Sayfalama
            var pagedDepartments = filteredDepartments
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();
            
            var departmentDtos = _mapper.Map<IEnumerable<DepartmentListDto>>(pagedDepartments);
            
            var result = new DepartmentSearchResultDto
            {
                Departments = departmentDtos,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = filter.Page,
                Filter = filter
            };
            
            return Result<DepartmentSearchResultDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return Result<DepartmentSearchResultDto>.ErrorResult($"Departman arama işlemi sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<DepartmentListDto>>> GetFilteredAsync(string? searchTerm, bool? isActive = null)
    {
        try
        {
            var filter = new DepartmentFilterDto
            {
                SearchTerm = searchTerm,
                IsActive = isActive,
                PageSize = 1000 // Tüm sonuçları getir
            };
            
            var searchResult = await SearchAsync(filter);
            
            if (searchResult.Success && searchResult.Data != null)
            {
                return Result<IEnumerable<DepartmentListDto>>.SuccessResult(searchResult.Data.Departments);
            }
            
            return Result<IEnumerable<DepartmentListDto>>.ErrorResult(searchResult.Message);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<DepartmentListDto>>.ErrorResult($"Departman filtreleme işlemi sırasında hata oluştu: {ex.Message}");
        }
    }
}
