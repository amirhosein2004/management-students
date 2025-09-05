using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagementAPI.Models;
using StudentManagementAPI.DTOs;

namespace StudentManagementAPI.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<IEnumerable<Student>> GetStudentsAsync(string searchTerm = null, string major = null);
        Task<(IEnumerable<Student> students, int totalCount)> GetStudentsPaginatedAsync(PaginationParameters parameters);
        Task<Student> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<bool> StudentExistsAsync(int id);
        Task<IEnumerable<string>> GetAllMajorsAsync();
    }
}