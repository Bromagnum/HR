using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<OrganizationListDto>>> GetAllAsync()
    {
        try
        {
            var organizations = await _unitOfWork.Organizations.GetAllAsync();
            var organizationDtos = _mapper.Map<IEnumerable<OrganizationListDto>>(organizations);
            
            return Result<IEnumerable<OrganizationListDto>>.Ok(organizationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<OrganizationListDto>>.Fail($"Organizations could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<OrganizationDetailDto>> GetByIdAsync(int id)
    {
        try
        {
            var organization = await _unitOfWork.Organizations.GetByIdAsync(id);
            if (organization == null)
            {
                return Result<OrganizationDetailDto>.Fail("Organization not found");
            }

            var organizationDto = _mapper.Map<OrganizationDetailDto>(organization);
            return Result<OrganizationDetailDto>.Ok(organizationDto);
        }
        catch (Exception ex)
        {
            return Result<OrganizationDetailDto>.Fail($"Organization could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<OrganizationDetailDto>> CreateAsync(OrganizationCreateDto dto)
    {
        try
        {
            var organization = _mapper.Map<Organization>(dto);
            organization.CreatedAt = DateTime.Now;
            organization.UpdatedAt = DateTime.Now;
            organization.IsActive = true;

            await _unitOfWork.Organizations.AddAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            var createdOrganization = await _unitOfWork.Organizations.GetByIdAsync(organization.Id);
            var organizationDto = _mapper.Map<OrganizationDetailDto>(createdOrganization);

            return Result<OrganizationDetailDto>.Ok(organizationDto);
        }
        catch (Exception ex)
        {
            return Result<OrganizationDetailDto>.Fail($"Organization could not be created: {ex.Message}");
        }
    }

    public async Task<Result<OrganizationDetailDto>> UpdateAsync(OrganizationUpdateDto dto)
    {
        try
        {
            var existingOrganization = await _unitOfWork.Organizations.GetByIdAsync(dto.Id);
            if (existingOrganization == null)
            {
                return Result<OrganizationDetailDto>.Fail("Organization not found");
            }

            _mapper.Map(dto, existingOrganization);
            existingOrganization.UpdatedAt = DateTime.Now;

            _unitOfWork.Organizations.Update(existingOrganization);
            await _unitOfWork.SaveChangesAsync();

            var updatedOrganization = await _unitOfWork.Organizations.GetByIdAsync(dto.Id);
            var organizationDto = _mapper.Map<OrganizationDetailDto>(updatedOrganization);

            return Result<OrganizationDetailDto>.Ok(organizationDto);
        }
        catch (Exception ex)
        {
            return Result<OrganizationDetailDto>.Fail($"Organization could not be updated: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var organization = await _unitOfWork.Organizations.GetByIdAsync(id);
            if (organization == null)
            {
                return Result<bool>.Fail("Organization not found");
            }

            // Soft delete
            organization.IsActive = false;
            organization.UpdatedAt = DateTime.Now;

            _unitOfWork.Organizations.Update(organization);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Organization could not be deleted: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<OrganizationListDto>>> GetFilteredAsync(OrganizationFilterDto filter)
    {
        try
        {
            var organizations = await _unitOfWork.Organizations.GetAllAsync();
            
            // Apply filters
            if (!string.IsNullOrEmpty(filter.Name))
                organizations = organizations.Where(o => o.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
                
            if (!string.IsNullOrEmpty(filter.Code))
                organizations = organizations.Where(o => o.Code.Contains(filter.Code, StringComparison.OrdinalIgnoreCase));
                
            if (filter.ParentOrganizationId.HasValue)
                organizations = organizations.Where(o => o.ParentOrganizationId == filter.ParentOrganizationId);
                
            if (!string.IsNullOrEmpty(filter.Manager))
                organizations = organizations.Where(o => o.Manager != null && o.Manager.Contains(filter.Manager, StringComparison.OrdinalIgnoreCase));
                
            if (filter.IsActive.HasValue)
                organizations = organizations.Where(o => o.IsActive == filter.IsActive.Value);

            // Apply pagination
            var pagedOrganizations = organizations
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize);

            var organizationDtos = _mapper.Map<IEnumerable<OrganizationListDto>>(pagedOrganizations);
            return Result<IEnumerable<OrganizationListDto>>.Ok(organizationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<OrganizationListDto>>.Fail($"Filtered organizations could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<OrganizationTreeDto>>> GetOrganizationTreeAsync()
    {
        try
        {
            var rootOrganizations = await _unitOfWork.Organizations.GetRootOrganizationsAsync();
            var treeDtos = _mapper.Map<IEnumerable<OrganizationTreeDto>>(rootOrganizations);
            
            return Result<IEnumerable<OrganizationTreeDto>>.Ok(treeDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<OrganizationTreeDto>>.Fail($"Organization tree could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<OrganizationListDto>>> GetRootOrganizationsAsync()
    {
        try
        {
            var organizations = await _unitOfWork.Organizations.GetRootOrganizationsAsync();
            var organizationDtos = _mapper.Map<IEnumerable<OrganizationListDto>>(organizations);
            
            return Result<IEnumerable<OrganizationListDto>>.Ok(organizationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<OrganizationListDto>>.Fail($"Root organizations could not be retrieved: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<OrganizationListDto>>> GetSubOrganizationsAsync(int parentId)
    {
        try
        {
            var organizations = await _unitOfWork.Organizations.GetSubOrganizationsAsync(parentId);
            var organizationDtos = _mapper.Map<IEnumerable<OrganizationListDto>>(organizations);
            
            return Result<IEnumerable<OrganizationListDto>>.Ok(organizationDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<OrganizationListDto>>.Fail($"Sub organizations could not be retrieved: {ex.Message}");
        }
    }
}
