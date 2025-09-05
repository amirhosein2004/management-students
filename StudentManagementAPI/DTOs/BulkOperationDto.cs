using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.DTOs
{
    public class BulkImportResultDto
    {
        public int TotalRecords { get; set; }
        public int SuccessfulImports { get; set; }
        public int FailedImports { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<StudentDto> ImportedStudents { get; set; } = new List<StudentDto>();
    }

    public class BulkDeleteDto
    {
        [Required(ErrorMessage = "حداقل یک دانشجو برای حذف انتخاب کنید")]
        public List<int> StudentIds { get; set; } = new List<int>();
    }

    public class BulkDeleteResultDto
    {
        public int TotalRequested { get; set; }
        public int SuccessfulDeletes { get; set; }
        public int FailedDeletes { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class StudentImportDto
    {
        [Required(ErrorMessage = "نام الزامی است")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "نام باید بین 2 تا 100 کاراکتر باشد")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "نام خانوادگی باید بین 2 تا 100 کاراکتر باشد")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "شماره دانشجویی الزامی است")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "شماره دانشجویی باید بین 5 تا 20 کاراکتر باشد")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "شماره دانشجویی فقط باید شامل اعداد باشد")]
        public string StudentNumber { get; set; }

        [Required(ErrorMessage = "رشته تحصیلی الزامی است")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "رشته تحصیلی باید بین 2 تا 50 کاراکتر باشد")]
        public string Major { get; set; }

        [Required(ErrorMessage = "سال ورود الزامی است")]
        [Range(1300, 1450, ErrorMessage = "سال ورود باید بین 1300 تا 1450 باشد")]
        public int EnrollmentYear { get; set; }

        // Optional fields for import
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
