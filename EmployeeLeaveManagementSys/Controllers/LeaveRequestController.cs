using EmployeeLeaveManagementSys.Data;
using EmployeeLeaveManagementSys.Models;
using AutoMapper;
using EmployeeLeaveManagementSys.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementSys.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LeaveRequestController(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Leave Application List
        public async Task<IActionResult> Index(
            string? searchString,
            int page = 1)
        {
            int pageSize = 5;

            var leaveQuery = _context.LeaveRequests
                .Include(l => l.Employee)
                .ThenInclude(e => e.Department)
                .AsQueryable();

            // Search by employee name
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                leaveQuery = leaveQuery.Where(l =>
                    l.Employee != null &&
                    l.Employee.Name.Contains(searchString));
            }

            var totalLeaves = await leaveQuery.CountAsync();

            var leaves = await leaveQuery
                .OrderByDescending(l => l.AppliedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;

            ViewBag.TotalPages = (int)Math.Ceiling(
                totalLeaves / (double)pageSize
            );

            ViewBag.SearchString = searchString;

            // Convert LeaveRequest entities to DTOs
            var leaveRequestDtos =
                _mapper.Map<List<LeaveRequestDto>>(leaves);

            return View(leaveRequestDtos);
        }

        // GET: Leave/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Employees = await _context.Employees
                .OrderBy(e => e.Name)
                .ToListAsync();

            return View();
        }

        // POST: Leave/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestDto leaveRequestDto)
        {
            // Validate date range
            if (leaveRequestDto.EndDate < leaveRequestDto.StartDate)
            {
                ModelState.AddModelError(
                    "EndDate",
                    "End date cannot be before start date.");
            }

            if (ModelState.IsValid)
            {
                // Convert DTO to Entity
                var leaveRequest =
                    _mapper.Map<LeaveRequest>(leaveRequestDto);

                // Set system-controlled values
                leaveRequest.Status = LeaveStatus.Pending;
                leaveRequest.AppliedDate = DateTime.Now;

                _context.LeaveRequests.Add(leaveRequest);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Reload employees if validation fails
            ViewBag.Employees = await _context.Employees
                .OrderBy(e => e.Name)
                .ToListAsync();

            return View(leaveRequestDto);
        }
    }
}