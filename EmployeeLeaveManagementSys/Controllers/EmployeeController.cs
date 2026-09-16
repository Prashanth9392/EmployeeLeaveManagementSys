using AutoMapper;
using EmployeeLeaveManagementSys.DTOs;
using EmployeeLeaveManagementSys.Data;
using EmployeeLeaveManagementSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementSys.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Employee List
        public async Task<IActionResult> Index(
            string? searchString,
            string? sortOrder,
            int page = 1)
        {
            int pageSize = 5;

            var employeeQuery = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                employeeQuery = employeeQuery
                    .Where(e => e.Name.Contains(searchString));
            }

            // Sorting
            ViewBag.NameSortParm =
                String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            ViewBag.EmailSortParm =
                sortOrder == "email" ? "email_desc" : "email";

            switch (sortOrder)
            {
                case "name_desc":
                    employeeQuery = employeeQuery
                        .OrderByDescending(e => e.Name);
                    break;

                case "email":
                    employeeQuery = employeeQuery
                        .OrderBy(e => e.Email);
                    break;

                case "email_desc":
                    employeeQuery = employeeQuery
                        .OrderByDescending(e => e.Email);
                    break;

                default:
                    employeeQuery = employeeQuery
                        .OrderBy(e => e.Name);
                    break;
            }

            var totalEmployees = await employeeQuery.CountAsync();

            var employees = await employeeQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;

            ViewBag.TotalPages = (int)Math.Ceiling(
                totalEmployees / (double)pageSize
            );

            ViewBag.SearchString = searchString;
            ViewBag.CurrentSort = sortOrder;

            // Convert Employee entities to Employee DTOs
            var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);

            return View(employeeDtos);
        }

        
        // GET: Employee/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = new SelectList(
                await _context.Departments.ToListAsync(),
                "Id",
                "Name");

            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                // Convert DTO to Employee entity
                var employee = _mapper.Map<Employee>(employeeDto);

                _context.Employees.Add(employee);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = new SelectList(
                await _context.Departments.ToListAsync(),
                "Id",
                "Name",
                employeeDto.DepartmentId);

            return View(employeeDto);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            // Convert Employee entity to DTO
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            ViewBag.Departments = new SelectList(
                await _context.Departments.ToListAsync(),
                "Id",
                "Name",
                employeeDto.DepartmentId);

            return View(employeeDto);
        }
        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeDto employeeDto)
        {
            if (id != employeeDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Convert DTO to Employee entity
                var employee = _mapper.Map<Employee>(employeeDto);

                _context.Update(employee);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = new SelectList(
                await _context.Departments.ToListAsync(),
                "Id",
                "Name",
                employeeDto.DepartmentId);

            return View(employeeDto);
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            // Convert Employee entity to DTO
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return View(employeeDto);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}