using AutoMapper;
using EmployeeLeaveManagementSys.Models;

namespace EmployeeLeaveManagementSys.DTOs
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();
        }
    }
}