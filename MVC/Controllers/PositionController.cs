using AutoMapper;
using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin,Manager")] // Sadece Admin ve Manager pozisyon yönetebilir
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public PositionController(IPositionService positionService, IDepartmentService departmentService, IMapper mapper)
        {
            _positionService = positionService;
            _departmentService = departmentService;
            _mapper = mapper;
        }

        // GET: Position
        public async Task<IActionResult> Index()
        {
            var result = await _positionService.GetAllAsync();
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(new List<PositionListViewModel>());
            }

            var viewModel = _mapper.Map<List<PositionListViewModel>>(result.Data);
            return View(viewModel);
        }

        // GET: Position/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = await _positionService.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return NotFound();
            }

            var viewModel = _mapper.Map<PositionDetailViewModel>(result.Data);
            return View(viewModel);
        }

        // GET: Position/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new PositionCreateViewModel();
            await LoadSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // POST: Position/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositionCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync(viewModel);
                return View(viewModel);
            }

            var dto = _mapper.Map<PositionCreateDto>(viewModel);
            var result = await _positionService.CreateAsync(dto);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Message;
            await LoadSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // GET: Position/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _positionService.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return NotFound();
            }

            var viewModel = _mapper.Map<PositionUpdateViewModel>(result.Data);
            await LoadSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // POST: Position/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PositionUpdateViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync(viewModel);
                return View(viewModel);
            }

            var dto = _mapper.Map<PositionUpdateDto>(viewModel);
            var result = await _positionService.UpdateAsync(dto);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Message;
            await LoadSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // GET: Position/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _positionService.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return NotFound();
            }

            var viewModel = _mapper.Map<PositionDetailViewModel>(result.Data);
            return View(viewModel);
        }

        // POST: Position/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _positionService.DeleteAsync(id);
            
            if (result.Success)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Position/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _positionService.ChangeStatusAsync(id);
            
            if (result.Success)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Position/ByDepartment/5
        public async Task<IActionResult> ByDepartment(int departmentId)
        {
            var result = await _positionService.GetByDepartmentIdAsync(departmentId);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("Index", new List<PositionListViewModel>());
            }

            var viewModel = _mapper.Map<List<PositionListViewModel>>(result.Data);
            
            // Get department name for display
            var departmentResult = await _departmentService.GetByIdAsync(departmentId);
            if (departmentResult.Success)
            {
                ViewBag.DepartmentName = departmentResult.Data.Name;
            }

            ViewBag.Title = "Departman Pozisyonları";
            return View("Index", viewModel);
        }

        // GET: Position/Available
        public async Task<IActionResult> Available()
        {
            var result = await _positionService.GetAvailablePositionsAsync();
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("Index", new List<PositionListViewModel>());
            }

            var viewModel = _mapper.Map<List<PositionListViewModel>>(result.Data);
            ViewBag.Title = "Müsait Pozisyonlar";
            return View("Index", viewModel);
        }

        // GET: Position/ByLevel
        public async Task<IActionResult> ByLevel(string level)
        {
            if (string.IsNullOrEmpty(level))
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _positionService.GetByLevelAsync(level);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("Index", new List<PositionListViewModel>());
            }

            var viewModel = _mapper.Map<List<PositionListViewModel>>(result.Data);
            ViewBag.Title = $"{level} Seviyesi Pozisyonlar";
            return View("Index", viewModel);
        }

        // GET: Position/ByEmploymentType
        public async Task<IActionResult> ByEmploymentType(string employmentType)
        {
            if (string.IsNullOrEmpty(employmentType))
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _positionService.GetByEmploymentTypeAsync(employmentType);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("Index", new List<PositionListViewModel>());
            }

            var viewModel = _mapper.Map<List<PositionListViewModel>>(result.Data);
            ViewBag.Title = $"{employmentType} Pozisyonları";
            return View("Index", viewModel);
        }

        // GET: Position/Debug
        public async Task<IActionResult> Debug()
        {
            var positions = await _positionService.GetAllAsync();
            var departments = await _departmentService.GetAllAsync();
            
            var debugInfo = new
            {
                PositionCount = positions.Data?.Count() ?? 0,
                DepartmentCount = departments.Data?.Count() ?? 0,
                Positions = positions.Data,
                Departments = departments.Data
            };
            
            return Json(debugInfo);
        }

        private async Task LoadSelectListsAsync(PositionCreateViewModel viewModel)
        {
            // Load departments
            var departmentResult = await _departmentService.GetAllAsync();
            if (departmentResult.Success)
            {
                viewModel.Departments = departmentResult.Data.Where(d => d.IsActive)
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    }).ToList();
            }

            // Load employment types
            viewModel.EmploymentTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "Tam Zamanlı", Text = "Tam Zamanlı" },
                new SelectListItem { Value = "Yarı Zamanlı", Text = "Yarı Zamanlı" },
                new SelectListItem { Value = "Sözleşmeli", Text = "Sözleşmeli" },
                new SelectListItem { Value = "Proje Bazlı", Text = "Proje Bazlı" },
                new SelectListItem { Value = "Stajyer", Text = "Stajyer" }
            };

            // Load levels
            viewModel.Levels = new List<SelectListItem>
            {
                new SelectListItem { Value = "Stajyer", Text = "Stajyer" },
                new SelectListItem { Value = "Junior", Text = "Junior" },
                new SelectListItem { Value = "Mid-Level", Text = "Mid-Level" },
                new SelectListItem { Value = "Senior", Text = "Senior" },
                new SelectListItem { Value = "Lead", Text = "Lead" },
                new SelectListItem { Value = "Manager", Text = "Manager" },
                new SelectListItem { Value = "Director", Text = "Director" }
            };
        }

        private async Task LoadSelectListsAsync(PositionUpdateViewModel viewModel)
        {
            // Load departments
            var departmentResult = await _departmentService.GetAllAsync();
            if (departmentResult.Success)
            {
                viewModel.Departments = departmentResult.Data.Where(d => d.IsActive)
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name,
                        Selected = d.Id == viewModel.DepartmentId
                    }).ToList();
            }

            // Load employment types
            viewModel.EmploymentTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "Tam Zamanlı", Text = "Tam Zamanlı", Selected = viewModel.EmploymentType == "Tam Zamanlı" },
                new SelectListItem { Value = "Yarı Zamanlı", Text = "Yarı Zamanlı", Selected = viewModel.EmploymentType == "Yarı Zamanlı" },
                new SelectListItem { Value = "Sözleşmeli", Text = "Sözleşmeli", Selected = viewModel.EmploymentType == "Sözleşmeli" },
                new SelectListItem { Value = "Proje Bazlı", Text = "Proje Bazlı", Selected = viewModel.EmploymentType == "Proje Bazlı" },
                new SelectListItem { Value = "Stajyer", Text = "Stajyer", Selected = viewModel.EmploymentType == "Stajyer" }
            };

            // Load levels
            viewModel.Levels = new List<SelectListItem>
            {
                new SelectListItem { Value = "Stajyer", Text = "Stajyer", Selected = viewModel.Level == "Stajyer" },
                new SelectListItem { Value = "Junior", Text = "Junior", Selected = viewModel.Level == "Junior" },
                new SelectListItem { Value = "Mid-Level", Text = "Mid-Level", Selected = viewModel.Level == "Mid-Level" },
                new SelectListItem { Value = "Senior", Text = "Senior", Selected = viewModel.Level == "Senior" },
                new SelectListItem { Value = "Lead", Text = "Lead", Selected = viewModel.Level == "Lead" },
                new SelectListItem { Value = "Manager", Text = "Manager", Selected = viewModel.Level == "Manager" },
                new SelectListItem { Value = "Director", Text = "Director", Selected = viewModel.Level == "Director" }
            };
        }
    }
}
