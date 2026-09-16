using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementSys.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
    }
}