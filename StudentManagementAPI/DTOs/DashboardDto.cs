using System.Collections.Generic;

namespace StudentManagementAPI.DTOs
{
    public class DashboardDto
    {
        public int TotalStudents { get; set; }
        public int TotalMajors { get; set; }
        public int StudentsThisYear { get; set; }
        public int StudentsLastYear { get; set; }
        public List<MajorStatistic> MajorStatistics { get; set; } = new List<MajorStatistic>();
        public List<EnrollmentStatistic> EnrollmentStatistics { get; set; } = new List<EnrollmentStatistic>();
        public List<RecentStudent> RecentStudents { get; set; } = new List<RecentStudent>();
    }

    public class MajorStatistic
    {
        public string Major { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class EnrollmentStatistic
    {
        public int Year { get; set; }
        public int Count { get; set; }
    }

    public class RecentStudent
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Major { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
