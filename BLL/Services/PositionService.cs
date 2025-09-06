using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services
{
    public class PositionService : IPositionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PositionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PositionListDto>>> GetAllAsync()
        {
            try
            {
                var positions = await _unitOfWork.Positions.GetPositionsWithPersonCountAsync();
                var positionDtos = _mapper.Map<IEnumerable<PositionListDto>>(positions);
                
                return Result<IEnumerable<PositionListDto>>.Ok(positionDtos, "Pozisyonlar başarıyla getirildi");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PositionListDto>>.Fail($"Pozisyonlar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<PositionDetailDto>> GetByIdAsync(int id)
        {
            try
            {
                var position = await _unitOfWork.Positions.GetPositionWithDepartmentAsync(id);
                if (position == null)
                {
                    return Result<PositionDetailDto>.Fail("Pozisyon bulunamadı");
                }

                var positionDto = _mapper.Map<PositionDetailDto>(position);
                return Result<PositionDetailDto>.Ok(positionDto, "Pozisyon başarıyla getirildi");
            }
            catch (Exception ex)
            {
                return Result<PositionDetailDto>.Fail($"Pozisyon getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<PositionDetailDto>> CreateAsync(PositionCreateDto dto)
        {
            try
            {
                // Department validation
                var department = await _unitOfWork.Departments.GetByIdAsync(dto.DepartmentId);
                if (department == null)
                {
                    return Result<PositionDetailDto>.Fail("Geçersiz departman seçimi");
                }

                // Salary validation
                if (dto.MinSalary.HasValue && dto.MaxSalary.HasValue && dto.MinSalary > dto.MaxSalary)
                {
                    return Result<PositionDetailDto>.Fail("Minimum maaş, maksimum maaştan büyük olamaz");
                }

                // Check if position name already exists in the same department
                var existingPositions = await _unitOfWork.Positions.GetByDepartmentIdAsync(dto.DepartmentId);
                if (existingPositions.Any(p => p.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase) && p.IsActive))
                {
                    return Result<PositionDetailDto>.Fail("Bu departmanda aynı isimde bir pozisyon zaten mevcut");
                }

                var position = _mapper.Map<Position>(dto);
                position.CreatedAt = DateTime.Now;
                position.UpdatedAt = DateTime.Now;
                position.IsActive = true;

                await _unitOfWork.Positions.AddAsync(position);
                await _unitOfWork.SaveChangesAsync();

                var createdPosition = await _unitOfWork.Positions.GetPositionWithDepartmentAsync(position.Id);
                var positionDto = _mapper.Map<PositionDetailDto>(createdPosition);

                return Result<PositionDetailDto>.Ok(positionDto, "Pozisyon başarıyla oluşturuldu");
            }
            catch (Exception ex)
            {
                return Result<PositionDetailDto>.Fail($"Pozisyon oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<PositionDetailDto>> UpdateAsync(PositionUpdateDto dto)
        {
            try
            {
                var position = await _unitOfWork.Positions.GetByIdAsync(dto.Id);
                if (position == null)
                {
                    return Result<PositionDetailDto>.Fail("Pozisyon bulunamadı");
                }

                // Department validation
                var department = await _unitOfWork.Departments.GetByIdAsync(dto.DepartmentId);
                if (department == null)
                {
                    return Result<PositionDetailDto>.Fail("Geçersiz departman seçimi");
                }

                // Salary validation
                if (dto.MinSalary.HasValue && dto.MaxSalary.HasValue && dto.MinSalary > dto.MaxSalary)
                {
                    return Result<PositionDetailDto>.Fail("Minimum maaş, maksimum maaştan büyük olamaz");
                }

                // Check if position name already exists in the same department (excluding current position)
                var existingPositions = await _unitOfWork.Positions.GetByDepartmentIdAsync(dto.DepartmentId);
                if (existingPositions.Any(p => p.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase) && p.IsActive && p.Id != dto.Id))
                {
                    return Result<PositionDetailDto>.Fail("Bu departmanda aynı isimde bir pozisyon zaten mevcut");
                }

                _mapper.Map(dto, position);
                position.UpdatedAt = DateTime.Now;

                _unitOfWork.Positions.Update(position);
                await _unitOfWork.SaveChangesAsync();

                var updatedPosition = await _unitOfWork.Positions.GetPositionWithDepartmentAsync(position.Id);
                var positionDto = _mapper.Map<PositionDetailDto>(updatedPosition);

                return Result<PositionDetailDto>.Ok(positionDto, "Pozisyon başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                return Result<PositionDetailDto>.Fail($"Pozisyon güncellenirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var position = await _unitOfWork.Positions.GetPositionWithDepartmentAsync(id);
                if (position == null)
                {
                    return Result.Fail("Pozisyon bulunamadı");
                }

                // Check if position has assigned persons
                if (position.Persons != null && position.Persons.Any(p => p.IsActive))
                {
                    return Result.Fail("Bu pozisyona atanmış aktif personel bulunduğu için silinemez");
                }

                _unitOfWork.Positions.Remove(position);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok("Pozisyon başarıyla silindi");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Pozisyon silinirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result> ChangeStatusAsync(int id)
        {
            try
            {
                var position = await _unitOfWork.Positions.GetByIdAsync(id);
                if (position == null)
                {
                    return Result.Fail("Pozisyon bulunamadı");
                }

                position.IsActive = !position.IsActive;
                position.UpdatedAt = DateTime.Now;

                _unitOfWork.Positions.Update(position);
                await _unitOfWork.SaveChangesAsync();

                var status = position.IsActive ? "aktif" : "pasif";
                return Result.Ok($"Pozisyon başarıyla {status} yapıldı");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Pozisyon durumu değiştirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PositionListDto>>> GetByDepartmentIdAsync(int departmentId)
        {
            try
            {
                var positions = await _unitOfWork.Positions.GetByDepartmentIdAsync(departmentId);
                var positionDtos = _mapper.Map<IEnumerable<PositionListDto>>(positions);
                
                return Result<IEnumerable<PositionListDto>>.Ok(positionDtos, "Departman pozisyonları başarıyla getirildi");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PositionListDto>>.Fail($"Departman pozisyonları getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PositionListDto>>> GetAvailablePositionsAsync()
        {
            try
            {
                var positions = await _unitOfWork.Positions.GetAvailablePositionsAsync();
                var positionDtos = _mapper.Map<IEnumerable<PositionListDto>>(positions);
                
                return Result<IEnumerable<PositionListDto>>.Ok(positionDtos, "Mevcut pozisyonlar başarıyla getirildi");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PositionListDto>>.Fail($"Mevcut pozisyonlar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PositionListDto>>> GetByLevelAsync(string level)
        {
            try
            {
                var positions = await _unitOfWork.Positions.GetByLevelAsync(level);
                var positionDtos = _mapper.Map<IEnumerable<PositionListDto>>(positions);
                
                return Result<IEnumerable<PositionListDto>>.Ok(positionDtos, $"{level} seviyesi pozisyonlar başarıyla getirildi");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PositionListDto>>.Fail($"{level} seviyesi pozisyonlar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PositionListDto>>> GetByEmploymentTypeAsync(string employmentType)
        {
            try
            {
                var positions = await _unitOfWork.Positions.GetByEmploymentTypeAsync(employmentType);
                var positionDtos = _mapper.Map<IEnumerable<PositionListDto>>(positions);
                
                return Result<IEnumerable<PositionListDto>>.Ok(positionDtos, $"{employmentType} türü pozisyonlar başarıyla getirildi");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PositionListDto>>.Fail($"{employmentType} türü pozisyonlar getirilirken hata oluştu: {ex.Message}");
            }
        }
    }
}
