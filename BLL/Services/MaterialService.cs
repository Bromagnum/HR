using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services;

public class MaterialService : IMaterialService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MaterialService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<MaterialListDto>>> GetAllAsync()
    {
        try
        {
            var materials = await _unitOfWork.Materials.GetAllAsync();
            var materialDtos = _mapper.Map<IEnumerable<MaterialListDto>>(materials);
            
            return Result<IEnumerable<MaterialListDto>>.Ok(materialDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<MaterialListDto>>.Fail($"Materials could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<MaterialDetailDto>> GetByIdAsync(int id)
    {
        try
        {
            var material = await _unitOfWork.Materials.GetByIdAsync(id);
            if (material == null)
            {
                return Result<MaterialDetailDto>.Fail("Material not found");
            }

            var materialDto = _mapper.Map<MaterialDetailDto>(material);
            return Result<MaterialDetailDto>.Ok(materialDto);
        }
        catch (Exception ex)
        {
            return Result<MaterialDetailDto>.Fail($"Material could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<MaterialDetailDto>> CreateAsync(MaterialCreateDto dto)
    {
        try
        {
            var material = _mapper.Map<Material>(dto);
            material.CreatedAt = DateTime.Now;
            material.UpdatedAt = DateTime.Now;
            material.IsActive = true;

            await _unitOfWork.Materials.AddAsync(material);
            await _unitOfWork.SaveChangesAsync();

            var createdMaterial = await _unitOfWork.Materials.GetByIdAsync(material.Id);
            var materialDto = _mapper.Map<MaterialDetailDto>(createdMaterial);

            return Result<MaterialDetailDto>.Ok(materialDto);
        }
        catch (Exception ex)
        {
            return Result<MaterialDetailDto>.Fail($"Material could not be created: {ex.Message}");
        }
    }

    public async Task<Result<MaterialDetailDto>> UpdateAsync(MaterialUpdateDto dto)
    {
        try
        {
            var existingMaterial = await _unitOfWork.Materials.GetByIdAsync(dto.Id);
            if (existingMaterial == null)
            {
                return Result<MaterialDetailDto>.Fail("Material not found");
            }

            _mapper.Map(dto, existingMaterial);
            existingMaterial.UpdatedAt = DateTime.Now;

            _unitOfWork.Materials.Update(existingMaterial);
            await _unitOfWork.SaveChangesAsync();

            var updatedMaterial = await _unitOfWork.Materials.GetByIdAsync(dto.Id);
            var materialDto = _mapper.Map<MaterialDetailDto>(updatedMaterial);

            return Result<MaterialDetailDto>.Ok(materialDto);
        }
        catch (Exception ex)
        {
            return Result<MaterialDetailDto>.Fail($"Material could not be updated: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var material = await _unitOfWork.Materials.GetByIdAsync(id);
            if (material == null)
            {
                return Result<bool>.Fail("Material not found");
            }

            // Soft delete
            material.IsActive = false;
            material.UpdatedAt = DateTime.Now;

            _unitOfWork.Materials.Update(material);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Material could not be deleted: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<MaterialListDto>>> GetFilteredAsync(MaterialFilterDto filter)
    {
        try
        {
            var materials = await _unitOfWork.Materials.GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(filter.Name))
                materials = materials.Where(m => m.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
                
            if (!string.IsNullOrEmpty(filter.Code))
                materials = materials.Where(m => m.Code.Contains(filter.Code, StringComparison.OrdinalIgnoreCase));
                
            if (!string.IsNullOrEmpty(filter.Category))
                materials = materials.Where(m => m.Category.Contains(filter.Category, StringComparison.OrdinalIgnoreCase));
                
            if (filter.OrganizationId.HasValue)
                materials = materials.Where(m => m.OrganizationId == filter.OrganizationId);
                
            if (filter.IsLowStock.HasValue && filter.IsLowStock.Value)
                materials = materials.Where(m => m.StockQuantity <= m.MinStockLevel);
                
            if (filter.IsActive.HasValue)
                materials = materials.Where(m => m.IsActive == filter.IsActive.Value);

            // Apply pagination
            var pagedMaterials = materials
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize);

            var materialDtos = _mapper.Map<IEnumerable<MaterialListDto>>(pagedMaterials);
            return Result<IEnumerable<MaterialListDto>>.Ok(materialDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<MaterialListDto>>.Fail($"Filtered materials could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<MaterialListDto>>> GetByOrganizationAsync(int organizationId)
    {
        try
        {
            var materials = await _unitOfWork.Materials.GetByOrganizationAsync(organizationId);
            var materialDtos = _mapper.Map<IEnumerable<MaterialListDto>>(materials);
            
            return Result<IEnumerable<MaterialListDto>>.Ok(materialDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<MaterialListDto>>.Fail($"Materials by organization could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<MaterialListDto>>> GetLowStockMaterialsAsync()
    {
        try
        {
            var materials = await _unitOfWork.Materials.GetLowStockMaterialsAsync();
            var materialDtos = _mapper.Map<IEnumerable<MaterialListDto>>(materials);
            
            return Result<IEnumerable<MaterialListDto>>.Ok(materialDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<MaterialListDto>>.Fail($"Low stock materials could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<MaterialListDto>>> GetByCategoryAsync(string category)
    {
        try
        {
            var materials = await _unitOfWork.Materials.GetByCategoryAsync(category);
            var materialDtos = _mapper.Map<IEnumerable<MaterialListDto>>(materials);
            
            return Result<IEnumerable<MaterialListDto>>.Ok(materialDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<MaterialListDto>>.Fail($"Materials by category could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<MaterialStockSummaryDto>> GetStockSummaryAsync()
    {
        try
        {
            var materials = await _unitOfWork.Materials.GetAllAsync();
            var activeMaterials = materials.Where(m => m.IsActive).ToList();

            var summary = new MaterialStockSummaryDto
            {
                TotalMaterials = activeMaterials.Count,
                LowStockCount = activeMaterials.Count(m => m.IsLowStock),
                OverStockCount = activeMaterials.Count(m => m.IsOverStock),
                TotalStockValue = activeMaterials.Sum(m => m.TotalValue),
                CategorySummary = activeMaterials
                    .GroupBy(m => m.Category)
                    .Select(g => new MaterialCategorySummaryDto
                    {
                        Category = g.Key,
                        Count = g.Count(),
                        TotalValue = g.Sum(m => m.TotalValue),
                        LowStockCount = g.Count(m => m.IsLowStock)
                    })
                    .ToList()
            };

            return Result<MaterialStockSummaryDto>.Ok(summary);
        }
        catch (Exception ex)
        {
            return Result<MaterialStockSummaryDto>.Fail($"Stock summary could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<string>>> GetCategoriesAsync()
    {
        try
        {
            var materials = await _unitOfWork.Materials.GetAllAsync();
            var categories = materials
                .Where(m => m.IsActive)
                .Select(m => m.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return Result<IEnumerable<string>>.Ok(categories);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<string>>.Fail($"Categories could not be retrieved: {ex.Message}");
        }
    }
}
