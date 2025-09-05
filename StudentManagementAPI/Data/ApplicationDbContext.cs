using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StudentNumber).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.StudentNumber).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.Major).HasMaxLength(50);
                entity.Property(e => e.EnrollmentYear).IsRequired();
            });

            // Seed data
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    FirstName = "علی",
                    LastName = "احمدی",
                    Email = "ali.ahmadi@university.edu",
                    PhoneNumber = "09121234567",
                    DateOfBirth = new System.DateTime(1380, 5, 15),
                    Address = "تهران، خیابان ولیعصر",
                    EnrollmentDate = new System.DateTime(1402, 9, 1),
                    StudentNumber = "4001001",
                    EnrollmentYear = 1402,
                    Major = "مهندسی کامپیوتر"
                },
                new Student
                {
                    Id = 2,
                    FirstName = "فاطمه",
                    LastName = "محمدی",
                    Email = "fateme.mohammadi@university.edu",
                    PhoneNumber = "09129876543",
                    DateOfBirth = new System.DateTime(1381, 8, 22),
                    Address = "تهران، خیابان انقلاب",
                    EnrollmentDate = new System.DateTime(1402, 9, 1),
                    StudentNumber = "4001002",
                    EnrollmentYear = 1402,
                    Major = "ریاضی"
                },
                new Student
                {
                    Id = 3,
                    FirstName = "محمد",
                    LastName = "رضایی",
                    Email = "mohammad.rezaei@university.edu",
                    PhoneNumber = "09124567890",
                    DateOfBirth = new System.DateTime(1379, 3, 10),
                    Address = "تهران، خیابان کریمخان",
                    EnrollmentDate = new System.DateTime(1402, 9, 1),
                    StudentNumber = "4001003",
                    EnrollmentYear = 1402,
                    Major = "فیزیک"
                }
            );
        }
    }
}