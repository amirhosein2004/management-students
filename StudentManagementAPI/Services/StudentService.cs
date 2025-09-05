using AutoMapper;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Models;
using StudentManagementAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagementAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<StudentDTO> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
                return null;

            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> CreateStudentAsync(CreateStudentDTO createStudentDto)
        {
            var student = _mapper.Map<Student>(createStudentDto);
            await _studentRepository.CreateStudentAsync(student);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> UpdateStudentAsync(int id, UpdateStudentDTO updateStudentDto)
        {
            var existingStudent = await _studentRepository.GetStudentByIdAsync(id);
            if (existingStudent == null)
                return null;

            // Update only the properties that are provided in the DTO
            if (updateStudentDto.FirstName != null)
                existingStudent.FirstName = updateStudentDto.FirstName;
            if (updateStudentDto.LastName != null)
                existingStudent.LastName = updateStudentDto.LastName;
            if (updateStudentDto.Email != null)
                existingStudent.Email = updateStudentDto.Email;
            if (updateStudentDto.PhoneNumber != null)
                existingStudent.PhoneNumber = updateStudentDto.PhoneNumber;
            if (updateStudentDto.DateOfBirth.HasValue)
                existingStudent.DateOfBirth = updateStudentDto.DateOfBirth.Value;
            if (updateStudentDto.Address != null)
                existingStudent.Address = updateStudentDto.Address;
            if (updateStudentDto.EnrollmentDate.HasValue)
                existingStudent.EnrollmentDate = updateStudentDto.EnrollmentDate.Value;
            if (updateStudentDto.Major != null)
                existingStudent.Major = updateStudentDto.Major;

            await _studentRepository.UpdateStudentAsync(existingStudent);
            return _mapper.Map<StudentDTO>(existingStudent);
        }

        public async Task<IEnumerable<StudentDTO>> GetStudentsAsync(string searchTerm = null, string major = null)
        {
            var students = await _studentRepository.GetStudentsAsync(searchTerm, major);
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<PaginatedResult<StudentDTO>> GetStudentsPaginatedAsync(PaginationParameters parameters)
        {
            var (students, totalCount) = await _studentRepository.GetStudentsPaginatedAsync(parameters);
            var studentDtos = _mapper.Map<IEnumerable<StudentDTO>>(students);
            
            return new PaginatedResult<StudentDTO>
            {
                Items = studentDtos,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        public async Task<IEnumerable<string>> GetAllMajorsAsync()
        {
            return await _studentRepository.GetAllMajorsAsync();
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var allStudents = await _studentRepository.GetAllStudentsAsync();
            var studentsList = allStudents.ToList();
            var currentYear = DateTime.Now.Year;

            var dashboard = new DashboardDto
            {
                TotalStudents = studentsList.Count,
                TotalMajors = studentsList.Where(s => !string.IsNullOrEmpty(s.Major)).Select(s => s.Major).Distinct().Count(),
                StudentsThisYear = studentsList.Count(s => s.EnrollmentDate.Year == currentYear),
                StudentsLastYear = studentsList.Count(s => s.EnrollmentDate.Year == currentYear - 1)
            };

            // Major statistics
            var majorGroups = studentsList
                .Where(s => !string.IsNullOrEmpty(s.Major))
                .GroupBy(s => s.Major)
                .Select(g => new MajorStatistic
                {
                    Major = g.Key,
                    Count = g.Count(),
                    Percentage = Math.Round((double)g.Count() / dashboard.TotalStudents * 100, 1)
                })
                .OrderByDescending(m => m.Count)
                .ToList();

            dashboard.MajorStatistics = majorGroups;

            // Enrollment statistics by year
            var enrollmentGroups = studentsList
                .GroupBy(s => s.EnrollmentDate.Year)
                .Select(g => new EnrollmentStatistic
                {
                    Year = g.Key,
                    Count = g.Count()
                })
                .OrderBy(e => e.Year)
                .ToList();

            dashboard.EnrollmentStatistics = enrollmentGroups;

            // Recent students (last 5)
            var recentStudents = studentsList
                .OrderByDescending(s => s.EnrollmentDate)
                .Take(5)
                .Select(s => new RecentStudent
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Major = s.Major,
                    EnrollmentDate = s.EnrollmentDate
                })
                .ToList();

            dashboard.RecentStudents = recentStudents;

            return dashboard;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await _studentRepository.DeleteStudentAsync(id);
        }

        public async Task<BulkImportResultDto> BulkImportStudentsAsync(IEnumerable<StudentImportDto> students)
        {
            var result = new BulkImportResultDto();
            var studentList = students.ToList();
            result.TotalRecords = studentList.Count;

            var existingStudents = await _studentRepository.GetAllStudentsAsync();
            var existingStudentNumbers = existingStudents.Select(s => s.StudentNumber).ToHashSet();

            foreach (var studentDto in studentList)
            {
                try
                {
                    // Check for duplicate student number
                    if (existingStudentNumbers.Contains(studentDto.StudentNumber))
                    {
                        result.Errors.Add($"شماره دانشجویی {studentDto.StudentNumber} قبلاً ثبت شده است");
                        result.FailedImports++;
                        continue;
                    }

                    // Create student entity
                    var student = new Student
                    {
                        FirstName = studentDto.FirstName,
                        LastName = studentDto.LastName,
                        StudentNumber = studentDto.StudentNumber,
                        Major = studentDto.Major,
                        EnrollmentYear = studentDto.EnrollmentYear,
                        Email = !string.IsNullOrEmpty(studentDto.Email) ? studentDto.Email : $"{studentDto.FirstName.ToLower()}.{studentDto.LastName.ToLower()}@university.edu",
                        PhoneNumber = !string.IsNullOrEmpty(studentDto.PhoneNumber) ? studentDto.PhoneNumber : "000-000-0000",
                        Address = !string.IsNullOrEmpty(studentDto.Address) ? studentDto.Address : "آدرس نامشخص",
                        DateOfBirth = DateTime.Now.AddYears(-20),
                        EnrollmentDate = DateTime.Now
                    };

                    var createdStudent = await _studentRepository.CreateStudentAsync(student);
                    
                    // Map to StudentDto for result
                    var createdStudentDto = new StudentDto
                    {
                        Id = createdStudent.Id,
                        FirstName = createdStudent.FirstName,
                        LastName = createdStudent.LastName,
                        StudentNumber = createdStudent.StudentNumber,
                        Major = createdStudent.Major,
                        EnrollmentYear = createdStudent.EnrollmentYear
                    };

                    result.ImportedStudents.Add(createdStudentDto);
                    result.SuccessfulImports++;
                    existingStudentNumbers.Add(studentDto.StudentNumber);
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"خطا در ثبت دانشجو {studentDto.FirstName} {studentDto.LastName}: {ex.Message}");
                    result.FailedImports++;
                }
            }

            return result;
        }

        public async Task<BulkDeleteResultDto> BulkDeleteStudentsAsync(List<int> studentIds)
        {
            var result = new BulkDeleteResultDto();
            result.TotalRequested = studentIds.Count;

            foreach (var studentId in studentIds)
            {
                try
                {
                    var deleted = await _studentRepository.DeleteStudentAsync(studentId);
                    if (deleted)
                    {
                        result.SuccessfulDeletes++;
                    }
                    else
                    {
                        result.Errors.Add($"دانشجو با شناسه {studentId} یافت نشد");
                        result.FailedDeletes++;
                    }
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"خطا در حذف دانشجو با شناسه {studentId}: {ex.Message}");
                    result.FailedDeletes++;
                }
            }

            return result;
        }
    }
}