using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.DTOs
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string StudentNumber { get; set; }
        public int EnrollmentYear { get; set; }
        public string Major { get; set; }
    }
    
    // کلاس مورد نیاز برای View ها
    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        public string Major { get; set; }
        public int EnrollmentYear { get; set; }
    }

    public class CreateStudentDTO
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [StringLength(200)]
        public string Address { get; set; }
        
        [Required]
        public DateTime EnrollmentDate { get; set; }
        
        [StringLength(50)]
        public string Major { get; set; }
    }
    
    // کلاس مورد نیاز برای View ها
    public class StudentCreateDto
    {
        [Required(ErrorMessage = "نام الزامی است")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "نام باید بین 2 تا 100 کاراکتر باشد")]
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "نام خانوادگی باید بین 2 تا 100 کاراکتر باشد")]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "شماره دانشجویی الزامی است")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "شماره دانشجویی باید بین 5 تا 20 کاراکتر باشد")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "شماره دانشجویی فقط باید شامل اعداد باشد")]
        [Display(Name = "شماره دانشجویی")]
        public string StudentNumber { get; set; }
        
        [Required(ErrorMessage = "رشته تحصیلی الزامی است")]
        [Display(Name = "رشته تحصیلی")]
        public string Major { get; set; }
        
        [Required(ErrorMessage = "سال ورود الزامی است")]
        [Range(1300, 1450, ErrorMessage = "سال ورود باید بین 1300 تا 1450 باشد")]
        [Display(Name = "سال ورود")]
        public int EnrollmentYear { get; set; }
    }

    public class UpdateStudentDTO
    {
        [StringLength(100)]
        public string FirstName { get; set; }
        
        [StringLength(100)]
        public string LastName { get; set; }
        
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }
        
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(200)]
        public string Address { get; set; }
        
        public DateTime? EnrollmentDate { get; set; }
        
        [StringLength(50)]
        public string Major { get; set; }
    }
    
    // کلاس مورد نیاز برای View ها
    public class StudentUpdateDto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "نام الزامی است")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "نام باید بین 2 تا 100 کاراکتر باشد")]
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "نام خانوادگی الزامی است")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "نام خانوادگی باید بین 2 تا 100 کاراکتر باشد")]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "شماره دانشجویی الزامی است")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "شماره دانشجویی باید بین 5 تا 20 کاراکتر باشد")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "شماره دانشجویی فقط باید شامل اعداد باشد")]
        [Display(Name = "شماره دانشجویی")]
        public string StudentNumber { get; set; }
        
        [Display(Name = "رشته تحصیلی")]
        public string Major { get; set; }
        
        [Required(ErrorMessage = "سال ورود الزامی است")]
        [Range(1300, 1450, ErrorMessage = "سال ورود باید بین 1300 تا 1450 باشد")]
        [Display(Name = "سال ورود")]
        public int EnrollmentYear { get; set; }
    }
}