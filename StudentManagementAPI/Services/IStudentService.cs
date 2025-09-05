using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllStudentsAsync();
        Task<IEnumerable<StudentDTO>> GetStudentsAsync(string searchTerm = null, string major = null);
        Task<PaginatedResult<StudentDTO>> GetStudentsPaginatedAsync(PaginationParameters parameters);
        Task<StudentDTO> GetStudentByIdAsync(int id);
        Task<StudentDTO> CreateStudentAsync(CreateStudentDTO createStudentDto);
        Task<StudentDTO> UpdateStudentAsync(int id, UpdateStudentDTO updateStudentDto);
        Task<bool> DeleteStudentAsync(int id);
        Task<IEnumerable<string>> GetAllMajorsAsync();
        Task<DashboardDto> GetDashboardDataAsync();
        Task<BulkImportResultDto> BulkImportStudentsAsync(IEnumerable<StudentImportDto> students);
        Task<BulkDeleteResultDto> BulkDeleteStudentsAsync(List<int> studentIds);
    }
}