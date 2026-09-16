using EmployeeLeaveManagementSys.Data;
using EmployeeLeaveManagementSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EmployeeLeaveManagementSys.DTOs;

namespace EmployeeLeaveManagementSys.Controllers
{
    public class LeaveApprovalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LeaveApprovalController(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Leave Approval List
        public async Task<IActionResult> Index()
        {
            var leaves = await _context.LeaveRequests
                .Include(l => l.Employee)
                .ThenInclude(e => e!.Department)
                .OrderBy(l => l.Status)
                .ThenByDescending(l => l.AppliedDate)
                .ToListAsync();

            // Convert Entity to DTO
            var leaveDtos = _mapper.Map<List<LeaveRequestDto>>(leaves);

            return View(leaveDtos);
        }

        // Approve Leave
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            if (leaveRequest.Status != LeaveStatus.Pending)
            {
                return RedirectToAction(nameof(Index));
            }

            leaveRequest.Status = LeaveStatus.Approved;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Reject Leave
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            if (leaveRequest.Status != LeaveStatus.Pending)
            {
                return RedirectToAction(nameof(Index));
            }

            leaveRequest.Status = LeaveStatus.Rejected;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}