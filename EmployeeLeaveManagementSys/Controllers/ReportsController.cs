using EmployeeLeaveManagementSys.Data;
using EmployeeLeaveManagementSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementSys.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalEmployees = await _context.Employees.CountAsync();

            var totalDepartments = await _context.Departments.CountAsync();

            var totalLeaves = await _context.LeaveRequests.CountAsync();

            var pendingLeaves = await _context.LeaveRequests
                .CountAsync(l => l.Status == LeaveStatus.Pending);

            var approvedLeaves = await _context.LeaveRequests
                .CountAsync(l => l.Status == LeaveStatus.Approved);

            var rejectedLeaves = await _context.LeaveRequests
                .CountAsync(l => l.Status == LeaveStatus.Rejected);

            var leaveRequests = await _context.LeaveRequests
                .Include(l => l.Employee)
                .ThenInclude(e => e.Department)
                .OrderByDescending(l => l.AppliedDate)
                .ToListAsync();

            ViewBag.TotalEmployees = totalEmployees;
            ViewBag.TotalDepartments = totalDepartments;
            ViewBag.TotalLeaves = totalLeaves;
            ViewBag.PendingLeaves = pendingLeaves;
            ViewBag.ApprovedLeaves = approvedLeaves;
            ViewBag.RejectedLeaves = rejectedLeaves;

            return View(leaveRequests);
        }
    }
}