using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementSys.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select an employee")]
        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }

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

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}