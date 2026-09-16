using AutoMapper;
using EmployeeLeaveManagementSys.Models;

namespace EmployeeLeaveManagementSys.DTOs
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(
                    dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department != null
                        ? src.Department.Name
                        : null));

            CreateMap<EmployeeDto, Employee>();
        }
    }
}