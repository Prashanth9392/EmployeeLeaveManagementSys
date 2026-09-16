using AutoMapper;
using EmployeeLeaveManagementSys.DTOs;
using EmployeeLeaveManagementSys.Data;
using EmployeeLeaveManagementSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementSys.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentController(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Department List
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .OrderBy(d => d.Name)
                .ToListAsync();

            // Convert Department entities to DTOs
            var departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);

            return View(departmentDtos);
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentDto departmentDto)
        {
            if (ModelState.IsValid)
            {
                // Convert DTO to Department entity
                var department = _mapper.Map<Department>(departmentDto);

                _context.Departments.Add(department);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(departmentDto);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            // Convert Entity to DTO
            var departmentDto = _mapper.Map<DepartmentDto>(department);

            return View(departmentDto);
        }

        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            DepartmentDto departmentDto)
        {
            if (id != departmentDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Convert DTO to Entity
                var department = _mapper.Map<Department>(departmentDto);

                _context.Departments.Update(department);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(departmentDto);
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}