using System.ComponentModel.DataAnnotations;
using EmployeeLeaveManagementSys.Models;

namespace EmployeeLeaveManagementSys.DTOs
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select an employee")]
        public int EmployeeId { get; set; }

        public string? EmployeeName { get; set; }

        public string? EmployeeDepartmentName { get; set; }

        [Required(ErrorMessage = "Please select a leave type")]
        public string LeaveType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
        public string Reason { get; set; } = string.Empty;

        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        public DateTime AppliedDate { get; set; } = DateTime.Now;
    }
}
