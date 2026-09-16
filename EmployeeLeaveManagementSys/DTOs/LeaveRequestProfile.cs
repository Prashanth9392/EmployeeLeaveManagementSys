using AutoMapper;
using EmployeeLeaveManagementSys.Models;

namespace EmployeeLeaveManagementSys.DTOs
{
    public class LeaveRequestProfile : Profile
    {
        public LeaveRequestProfile()
        {
            CreateMap<LeaveRequest, LeaveRequestDto>()
                .ForMember(
                    dest => dest.EmployeeName,
                    opt => opt.MapFrom(src => src.Employee != null
                        ? src.Employee.Name
                        : null));

            CreateMap<LeaveRequest, LeaveRequestDto>()
                .ForMember(
                    dest => dest.EmployeeDepartmentName,
                    opt => opt.MapFrom(src => src.Employee != null && src.Employee.Department != null
                        ? src.Employee.Department.Name
                        : null));

            CreateMap<LeaveRequestDto, LeaveRequest>();
        }
    }
}
