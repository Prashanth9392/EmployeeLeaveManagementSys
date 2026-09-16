using EmployeeLeaveManagementSys.Data;
using EmployeeLeaveManagementSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementSys.Controllers
{
    public class ManagerDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerDashboardController(ApplicationDbContext context)
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

            var recentLeaves = await _context.LeaveRequests
                .Include(l => l.Employee)
                .OrderByDescending(l => l.AppliedDate)
                .Take(5)
                .ToListAsync();

            ViewBag.TotalEmployees = totalEmployees;
            ViewBag.TotalDepartments = totalDepartments;
            ViewBag.TotalLeaves = totalLeaves;
            ViewBag.PendingLeaves = pendingLeaves;
            ViewBag.ApprovedLeaves = approvedLeaves;
            ViewBag.RejectedLeaves = rejectedLeaves;

            return View(recentLeaves);
        }
    }
}