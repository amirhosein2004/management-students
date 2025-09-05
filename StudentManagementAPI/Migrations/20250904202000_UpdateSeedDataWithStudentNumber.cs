using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedDataWithStudentNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FirstName", "LastName", "Email", "PhoneNumber", "DateOfBirth", "Address", "EnrollmentDate", "StudentNumber", "EnrollmentYear", "Major" },
                values: new object[] { "علی", "احمدی", "ali.ahmadi@university.edu", "09121234567", new DateTime(2001, 5, 15), "تهران، خیابان ولیعصر", new DateTime(2023, 9, 1), "4001001", 1402, "مهندسی کامپیوتر" });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FirstName", "LastName", "Email", "PhoneNumber", "DateOfBirth", "Address", "EnrollmentDate", "StudentNumber", "EnrollmentYear", "Major" },
                values: new object[] { "فاطمه", "محمدی", "fateme.mohammadi@university.edu", "09129876543", new DateTime(2002, 8, 22), "تهران، خیابان انقلاب", new DateTime(2023, 9, 1), "4001002", 1402, "ریاضی" });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FirstName", "LastName", "Email", "PhoneNumber", "DateOfBirth", "Address", "EnrollmentDate", "StudentNumber", "EnrollmentYear", "Major" },
                values: new object[] { "محمد", "رضایی", "mohammad.rezaei@university.edu", "09124567890", new DateTime(2000, 3, 10), "تهران، خیابان کریمخان", new DateTime(2023, 9, 1), "4001003", 1402, "فیزیک" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FirstName", "LastName", "Email", "PhoneNumber", "DateOfBirth", "Address", "EnrollmentDate", "StudentNumber", "EnrollmentYear", "Major" },
                values: new object[] { "John", "Doe", "john.doe@example.com", "555-123-4567", new DateTime(1995, 5, 15), "123 Main St, Anytown, USA", new DateTime(2023, 9, 1), "", 0, "Computer Science" });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FirstName", "LastName", "Email", "PhoneNumber", "DateOfBirth", "Address", "EnrollmentDate", "StudentNumber", "EnrollmentYear", "Major" },
                values: new object[] { "Jane", "Smith", "jane.smith@example.com", "555-987-6543", new DateTime(1997, 8, 22), "456 Oak Ave, Somewhere, USA", new DateTime(2023, 9, 1), "", 0, "Mathematics" });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FirstName", "LastName", "Email", "PhoneNumber", "DateOfBirth", "Address", "EnrollmentDate", "StudentNumber", "EnrollmentYear", "Major" },
                values: new object[] { "Michael", "Johnson", "michael.johnson@example.com", "555-456-7890", new DateTime(1996, 3, 10), "789 Pine St, Nowhere, USA", new DateTime(2023, 9, 1), "", 0, "Physics" });
        }
    }
}
