using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Services;
using System.Threading.Tasks;

namespace StudentManagementAPI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IExportService _exportService;

        public DashboardController(IStudentService studentService, IExportService exportService)
        {
            _studentService = studentService;
            _exportService = exportService;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = await _studentService.GetDashboardDataAsync();
            return View(dashboardData);
        }

        // Export Dashboard to PDF
        public async Task<IActionResult> ExportDashboardToPdf()
        {
            try
            {
                var dashboardData = await _studentService.GetDashboardDataAsync();
                var pdfData = await _exportService.ExportDashboardToPdfAsync(dashboardData);

                var fileName = $"Dashboard_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                return File(pdfData, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در تولید گزارش داشبورد: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
