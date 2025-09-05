using AutoMapper;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map from Student to StudentDTO
            CreateMap<Student, StudentDTO>();
            
            // Map from CreateStudentDTO to Student
            CreateMap<CreateStudentDTO, Student>()
                .ForMember(dest => dest.StudentNumber, opt => opt.Ignore())
                .ForMember(dest => dest.EnrollmentYear, opt => opt.Ignore());
            
            // Map from UpdateStudentDTO to Student
            CreateMap<UpdateStudentDTO, Student>()
                .ForMember(dest => dest.StudentNumber, opt => opt.Ignore())
                .ForMember(dest => dest.EnrollmentYear, opt => opt.Ignore());
            
            // Map from StudentDTO to StudentDto (for views)
            CreateMap<StudentDTO, StudentDto>()
                .ForMember(dest => dest.StudentNumber, opt => opt.MapFrom(src => src.StudentNumber))
                .ForMember(dest => dest.EnrollmentYear, opt => opt.MapFrom(src => src.EnrollmentYear));
            
            // Map from StudentDTO to StudentUpdateDto (for views)
            CreateMap<StudentDTO, StudentUpdateDto>()
                .ForMember(dest => dest.StudentNumber, opt => opt.MapFrom(src => src.StudentNumber))
                .ForMember(dest => dest.EnrollmentYear, opt => opt.MapFrom(src => src.EnrollmentYear));
        }
    }
}