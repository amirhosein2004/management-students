using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        
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
        
        public DateTime DateOfBirth { get; set; }
        
        [StringLength(200)]
        public string Address { get; set; }
        
        [Required]
        public DateTime EnrollmentDate { get; set; }
        
        [Required]
        [StringLength(20)]
        public string StudentNumber { get; set; }
        
        [Required]
        public int EnrollmentYear { get; set; }
        
        [StringLength(50)]
        public string Major { get; set; }
    }
}