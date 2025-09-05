using StudentManagementAPI.DTOs;

namespace StudentManagementAPI.Services
{
    public interface IExportService
    {
        Task<byte[]> ExportStudentsToExcelAsync(IEnumerable<StudentDto> students);
        Task<byte[]> ExportStudentsToPdfAsync(IEnumerable<StudentDto> students);
        Task<byte[]> ExportDashboardToPdfAsync(DashboardDto dashboardData);
    }
}
