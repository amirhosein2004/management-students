using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Data;
using StudentManagementAPI.Models;
using StudentManagementAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace StudentManagementAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync(string searchTerm = null, string major = null)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(s => s.FirstName.Contains(searchTerm) || 
                                        s.LastName.Contains(searchTerm) || 
                                        s.Email.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(major))
            {
                query = query.Where(s => s.Major == major);
            }

            return await query.ToListAsync();
        }

        public async Task<(IEnumerable<Student> students, int totalCount)> GetStudentsPaginatedAsync(PaginationParameters parameters)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                query = query.Where(s => s.FirstName.Contains(parameters.SearchTerm) || 
                                        s.LastName.Contains(parameters.SearchTerm) || 
                                        s.Email.Contains(parameters.SearchTerm));
            }

            if (!string.IsNullOrEmpty(parameters.Major))
            {
                query = query.Where(s => s.Major == parameters.Major);
            }

            var totalCount = await query.CountAsync();
            
            var students = await query
                .OrderBy(s => s.Id)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (students, totalCount);
        }

        public async Task<IEnumerable<string>> GetAllMajorsAsync()
        {
            return await _context.Students
                .Where(s => !string.IsNullOrEmpty(s.Major))
                .Select(s => s.Major)
                .Distinct()
                .OrderBy(m => m)
                .ToListAsync();
        }

        public async Task<bool> StudentExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(e => e.Id == id);
        }
    }
}