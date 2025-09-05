using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Models;
using StudentManagementAPI.Services;
using StudentManagementAPI.DTOs;
using System.Threading.Tasks;
using AutoMapper;

namespace StudentManagementAPI.Controllers
{
    public class StudentsViewController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IExportService _exportService;

        public StudentsViewController(IStudentService studentService, IMapper mapper, IExportService exportService)
        {
            _studentService = studentService;
            _mapper = mapper;
            _exportService = exportService;
        }

        // GET: StudentsView
        public async Task<IActionResult> Index(string searchTerm, string major, int pageNumber = 1, int pageSize = 10)
        {
            var parameters = new PaginationParameters
            {
                SearchTerm = searchTerm,
                Major = major,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var paginatedResult = await _studentService.GetStudentsPaginatedAsync(parameters);
            var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(paginatedResult.Items);
            
            // Get all majors for the filter dropdown
            var majors = await _studentService.GetAllMajorsAsync();
            ViewBag.Majors = majors;
            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentMajor = major;
            ViewBag.PaginationInfo = paginatedResult;
            
            return View(studentDtos);
        }

        // GET: StudentsView/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            var studentDto = _mapper.Map<StudentDto>(student);
            return View(studentDto);
        }

        // GET: StudentsView/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentsView/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateDto studentDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // بررسی تکراری نبودن شماره دانشجویی
                    var existingStudents = await _studentService.GetAllStudentsAsync();
                    if (existingStudents.Any(s => s.Id.ToString() == studentDto.StudentNumber))
                    {
                        ModelState.AddModelError("StudentNumber", "این شماره دانشجویی قبلاً ثبت شده است");
                        return View(studentDto);
                    }

                    // تبدیل StudentCreateDto به CreateStudentDTO
                    var createStudentDTO = new CreateStudentDTO
                    {
                        FirstName = studentDto.FirstName,
                        LastName = studentDto.LastName,
                        Major = studentDto.Major,
                        Email = $"{studentDto.FirstName.ToLower()}.{studentDto.LastName.ToLower()}@university.edu",
                        PhoneNumber = "000-000-0000",
                        DateOfBirth = DateTime.Now.AddYears(-20),
                        EnrollmentDate = DateTime.Now,
                        Address = "آدرس نامشخص"
                    };
                    
                    await _studentService.CreateStudentAsync(createStudentDTO);
                    TempData["SuccessMessage"] = "دانشجو با موفقیت اضافه شد";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "خطا در ثبت دانشجو: " + ex.Message);
                }
            }
            return View(studentDto);
        }

        // GET: StudentsView/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            var studentDto = _mapper.Map<StudentUpdateDto>(student);
            return View(studentDto);
        }

        // POST: StudentsView/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentUpdateDto studentDto)
        {
            if (id != studentDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // بررسی تکراری نبودن شماره دانشجویی (به جز دانشجوی فعلی)
                    var existingStudents = await _studentService.GetAllStudentsAsync();
                    var duplicateStudent = existingStudents.FirstOrDefault(s => s.Id.ToString() == studentDto.StudentNumber && s.Id != id);
                    if (duplicateStudent != null)
                    {
                        ModelState.AddModelError("StudentNumber", "این شماره دانشجویی قبلاً ثبت شده است");
                        return View(studentDto);
                    }

                    // تبدیل StudentUpdateDto به UpdateStudentDTO
                    var updateStudentDTO = new UpdateStudentDTO
                    {
                        FirstName = studentDto.FirstName,
                        LastName = studentDto.LastName,
                        Major = studentDto.Major,
                        Email = $"{studentDto.FirstName.ToLower()}.{studentDto.LastName.ToLower()}@university.edu",
                        PhoneNumber = "000-000-0000"
                    };
                    
                    await _studentService.UpdateStudentAsync(id, updateStudentDTO);
                    TempData["SuccessMessage"] = "اطلاعات دانشجو با موفقیت به‌روزرسانی شد";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "خطا در به‌روزرسانی دانشجو: " + ex.Message);
                }
            }
            return View(studentDto);
        }

        // GET: StudentsView/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            var studentDto = _mapper.Map<StudentDto>(student);
            return View(studentDto);
        }

        // POST: StudentsView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Export to Excel
        public async Task<IActionResult> ExportToExcel(string searchTerm, string major)
        {
            try
            {
                // Get all students based on current filters
                var parameters = new PaginationParameters
                {
                    SearchTerm = searchTerm,
                    Major = major,
                    PageNumber = 1,
                    PageSize = int.MaxValue // Get all records
                };

                var paginatedResult = await _studentService.GetStudentsPaginatedAsync(parameters);
                var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(paginatedResult.Items);

                var excelData = await _exportService.ExportStudentsToExcelAsync(studentDtos);

                var fileName = $"Students_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در تولید فایل اکسل: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // Export to PDF
        public async Task<IActionResult> ExportToPdf(string searchTerm, string major)
        {
            try
            {
                // Get all students based on current filters
                var parameters = new PaginationParameters
                {
                    SearchTerm = searchTerm,
                    Major = major,
                    PageNumber = 1,
                    PageSize = int.MaxValue // Get all records
                };

                var paginatedResult = await _studentService.GetStudentsPaginatedAsync(parameters);
                var studentDtos = _mapper.Map<IEnumerable<StudentDto>>(paginatedResult.Items);

                var pdfData = await _exportService.ExportStudentsToPdfAsync(studentDtos);

                var fileName = $"Students_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                return File(pdfData, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در تولید فایل PDF: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: StudentsView/BulkImport
        public IActionResult BulkImport()
        {
            return View();
        }

        // POST: StudentsView/BulkImport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkImport(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "لطفاً فایل اکسل را انتخاب کنید";
                return View();
            }

            if (!file.FileName.EndsWith(".xlsx") && !file.FileName.EndsWith(".xls"))
            {
                TempData["ErrorMessage"] = "فقط فایل‌های اکسل (.xlsx, .xls) پشتیبانی می‌شوند";
                return View();
            }

            try
            {
                var students = new List<StudentImportDto>();

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    using (var package = new OfficeOpenXml.ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension?.Rows ?? 0;

                        if (rowCount < 2)
                        {
                            TempData["ErrorMessage"] = "فایل اکسل خالی است یا فرمت صحیح ندارد";
                            return View();
                        }

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var firstName = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                            var lastName = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                            var studentNumber = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                            var major = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                            var enrollmentYearStr = worksheet.Cells[row, 5].Value?.ToString()?.Trim();

                            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || 
                                string.IsNullOrEmpty(studentNumber) || string.IsNullOrEmpty(major) ||
                                string.IsNullOrEmpty(enrollmentYearStr))
                            {
                                continue; // Skip empty rows
                            }

                            if (int.TryParse(enrollmentYearStr, out int enrollmentYear))
                            {
                                students.Add(new StudentImportDto
                                {
                                    FirstName = firstName,
                                    LastName = lastName,
                                    StudentNumber = studentNumber,
                                    Major = major,
                                    EnrollmentYear = enrollmentYear,
                                    Email = worksheet.Cells[row, 6].Value?.ToString()?.Trim(),
                                    PhoneNumber = worksheet.Cells[row, 7].Value?.ToString()?.Trim(),
                                    Address = worksheet.Cells[row, 8].Value?.ToString()?.Trim()
                                });
                            }
                        }
                    }
                }

                if (!students.Any())
                {
                    TempData["ErrorMessage"] = "هیچ داده معتبری در فایل یافت نشد";
                    return View();
                }

                var result = await _studentService.BulkImportStudentsAsync(students);

                TempData["SuccessMessage"] = $"وارد کردن انجام شد. موفق: {result.SuccessfulImports}، ناموفق: {result.FailedImports}";
                
                if (result.Errors.Any())
                {
                    TempData["ImportErrors"] = result.Errors;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در پردازش فایل: " + ex.Message;
                return View();
            }
        }

        // POST: StudentsView/BulkDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteDto bulkDeleteDto)
        {
            if (bulkDeleteDto?.StudentIds == null || !bulkDeleteDto.StudentIds.Any())
            {
                return Json(new { success = false, message = "هیچ دانشجویی برای حذف انتخاب نشده است" });
            }

            try
            {
                var result = await _studentService.BulkDeleteStudentsAsync(bulkDeleteDto.StudentIds);
                
                return Json(new 
                { 
                    success = true, 
                    message = $"حذف انجام شد. موفق: {result.SuccessfulDeletes}، ناموفق: {result.FailedDeletes}",
                    successfulDeletes = result.SuccessfulDeletes,
                    failedDeletes = result.FailedDeletes,
                    errors = result.Errors
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "خطا در حذف دانشجویان: " + ex.Message });
            }
        }
    }
}