using EmployeeLeaveManagementSys.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementSys.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }
    }
}