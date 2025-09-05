using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using StudentManagementAPI.DTOs;
using System.Text;

namespace StudentManagementAPI.Services
{
    public class ExportService : IExportService
    {
        public async Task<byte[]> ExportStudentsToExcelAsync(IEnumerable<StudentDto> students)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("دانشجویان");

            // Set RTL for Persian text
            worksheet.View.RightToLeft = true;

            // Headers
            worksheet.Cells[1, 1].Value = "شناسه";
            worksheet.Cells[1, 2].Value = "نام";
            worksheet.Cells[1, 3].Value = "نام خانوادگی";
            worksheet.Cells[1, 4].Value = "شماره دانشجویی";
            worksheet.Cells[1, 5].Value = "رشته تحصیلی";
            worksheet.Cells[1, 6].Value = "سال ورود";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
            }

            // Data
            var studentList = students.ToList();
            for (int i = 0; i < studentList.Count; i++)
            {
                var student = studentList[i];
                worksheet.Cells[i + 2, 1].Value = student.Id;
                worksheet.Cells[i + 2, 2].Value = student.FirstName;
                worksheet.Cells[i + 2, 3].Value = student.LastName;
                worksheet.Cells[i + 2, 4].Value = student.StudentNumber;
                worksheet.Cells[i + 2, 5].Value = student.Major;
                worksheet.Cells[i + 2, 6].Value = student.EnrollmentYear;
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();

            // Add borders to all data
            using (var range = worksheet.Cells[1, 1, studentList.Count + 1, 6])
            {
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }

            // Add summary row
            var summaryRow = studentList.Count + 3;
            worksheet.Cells[summaryRow, 1].Value = "تعداد کل دانشجویان:";
            worksheet.Cells[summaryRow, 2].Value = studentList.Count;
            worksheet.Cells[summaryRow, 1, summaryRow, 2].Style.Font.Bold = true;

            return await Task.FromResult(package.GetAsByteArray());
        }

        public async Task<byte[]> ExportStudentsToPdfAsync(IEnumerable<StudentDto> students)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, memoryStream);
            
            document.Open();

            // Create Persian font
            var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            BaseFont baseFont;
            
            try
            {
                baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            }
            catch
            {
                // Fallback to built-in font
                baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            }

            var titleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
            var headerFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);
            var normalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL);

            // Title
            var title = new Paragraph("گزارش جامع دانشجویان دانشگاه", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 10;
            document.Add(title);

            // Subtitle
            var subtitle = new Paragraph("سیستم مدیریت دانشجویان", headerFont);
            subtitle.Alignment = Element.ALIGN_CENTER;
            subtitle.SpacingAfter = 15;
            document.Add(subtitle);

            // Date and info
            var dateInfo = new Paragraph($"تاریخ تولید گزارش: {DateTime.Now:yyyy/MM/dd HH:mm} | تعداد کل دانشجویان: {students.Count()}", normalFont);
            dateInfo.Alignment = Element.ALIGN_RIGHT;
            dateInfo.SpacingAfter = 20;
            document.Add(dateInfo);

            // Create table
            var table = new PdfPTable(6);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 1, 2, 2, 2, 3, 1.5f });

            // Headers
            var headers = new[] { "شناسه", "نام", "نام خانوادگی", "شماره دانشجویی", "رشته تحصیلی", "سال ورود" };
            foreach (var header in headers)
            {
                var cell = new PdfPCell(new Phrase(header, headerFont));
                cell.BackgroundColor = new BaseColor(240, 240, 240);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                table.AddCell(cell);
            }

            // Data rows
            var studentList = students.ToList();
            foreach (var student in studentList)
            {
                table.AddCell(new PdfPCell(new Phrase(student.Id.ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(student.FirstName ?? "", normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(student.LastName ?? "", normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(student.StudentNumber ?? "", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(student.Major ?? "", normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 3 });
                table.AddCell(new PdfPCell(new Phrase(student.EnrollmentYear.ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 3 });
            }

            document.Add(table);

            // Summary
            var summary = new Paragraph($"\nتعداد کل دانشجویان: {studentList.Count}", headerFont);
            summary.Alignment = Element.ALIGN_RIGHT;
            summary.SpacingBefore = 20;
            document.Add(summary);

            document.Close();
            return await Task.FromResult(memoryStream.ToArray());
        }

        public async Task<byte[]> ExportDashboardToPdfAsync(DashboardDto dashboardData)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, memoryStream);
            
            document.Open();

            // Create Persian font
            var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            BaseFont baseFont;
            
            try
            {
                baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            }
            catch
            {
                baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            }

            var titleFont = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD);
            var headerFont = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD);
            var normalFont = new iTextSharp.text.Font(baseFont, 11, iTextSharp.text.Font.NORMAL);

            // Title
            var title = new Paragraph("گزارش تحلیلی داشبورد دانشگاه", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 10;
            document.Add(title);

            // Subtitle
            var subtitle = new Paragraph("آمار و اطلاعات جامع دانشجویان", headerFont);
            subtitle.Alignment = Element.ALIGN_CENTER;
            subtitle.SpacingAfter = 15;
            document.Add(subtitle);

            // Date and period info
            var dateInfo = new Paragraph($"تاریخ تولید گزارش: {DateTime.Now:yyyy/MM/dd HH:mm} | دوره گزارش: سال تحصیلی جاری", normalFont);
            dateInfo.Alignment = Element.ALIGN_RIGHT;
            dateInfo.SpacingAfter = 20;
            document.Add(dateInfo);

            // Statistics Summary
            var statsHeader = new Paragraph("خلاصه آمار", headerFont);
            statsHeader.Alignment = Element.ALIGN_RIGHT;
            statsHeader.SpacingAfter = 10;
            document.Add(statsHeader);

            var statsTable = new PdfPTable(2);
            statsTable.WidthPercentage = 100;
            statsTable.SetWidths(new float[] { 3, 1 });

            // Add statistics
            var stats = new[]
            {
                ("کل دانشجویان", dashboardData.TotalStudents.ToString()),
                ("تعداد رشته‌های تحصیلی", dashboardData.TotalMajors.ToString()),
                ("دانشجویان امسال", dashboardData.StudentsThisYear.ToString()),
                ("دانشجویان پارسال", dashboardData.StudentsLastYear.ToString())
            };

            foreach (var (label, value) in stats)
            {
                statsTable.AddCell(new PdfPCell(new Phrase(label, normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });
                statsTable.AddCell(new PdfPCell(new Phrase(value, normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5 });
            }

            document.Add(statsTable);

            // Major Statistics
            if (dashboardData.MajorStatistics?.Any() == true)
            {
                var majorHeader = new Paragraph("\nآمار رشته‌های تحصیلی", headerFont);
                majorHeader.Alignment = Element.ALIGN_RIGHT;
                majorHeader.SpacingAfter = 10;
                document.Add(majorHeader);

                var majorTable = new PdfPTable(3);
                majorTable.WidthPercentage = 100;
                majorTable.SetWidths(new float[] { 3, 1, 1 });

                // Headers
                majorTable.AddCell(new PdfPCell(new Phrase("رشته تحصیلی", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = new BaseColor(240, 240, 240), Padding = 5 });
                majorTable.AddCell(new PdfPCell(new Phrase("تعداد", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = new BaseColor(240, 240, 240), Padding = 5 });
                majorTable.AddCell(new PdfPCell(new Phrase("درصد", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = new BaseColor(240, 240, 240), Padding = 5 });

                foreach (var major in dashboardData.MajorStatistics)
                {
                    majorTable.AddCell(new PdfPCell(new Phrase(major.Major, normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 3 });
                    majorTable.AddCell(new PdfPCell(new Phrase(major.Count.ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 3 });
                    majorTable.AddCell(new PdfPCell(new Phrase($"{major.Percentage}%", normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 3 });
                }

                document.Add(majorTable);
            }

            // Recent Students
            if (dashboardData.RecentStudents?.Any() == true)
            {
                var recentHeader = new Paragraph("\nآخرین دانشجویان ثبت‌نام شده", headerFont);
                recentHeader.Alignment = Element.ALIGN_RIGHT;
                recentHeader.SpacingAfter = 10;
                document.Add(recentHeader);

                var recentTable = new PdfPTable(3);
                recentTable.WidthPercentage = 100;
                recentTable.SetWidths(new float[] { 2, 2, 2 });

                // Headers
                recentTable.AddCell(new PdfPCell(new Phrase("نام و نام خانوادگی", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = new BaseColor(240, 240, 240), Padding = 5 });
                recentTable.AddCell(new PdfPCell(new Phrase("رشته تحصیلی", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = new BaseColor(240, 240, 240), Padding = 5 });
                recentTable.AddCell(new PdfPCell(new Phrase("تاریخ ثبت‌نام", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = new BaseColor(240, 240, 240), Padding = 5 });

                foreach (var student in dashboardData.RecentStudents)
                {
                    recentTable.AddCell(new PdfPCell(new Phrase($"{student.FirstName} {student.LastName}", normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 3 });
                    recentTable.AddCell(new PdfPCell(new Phrase(student.Major, normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 3 });
                    recentTable.AddCell(new PdfPCell(new Phrase(student.EnrollmentDate.ToString("yyyy/MM/dd"), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 3 });
                }

                document.Add(recentTable);
            }

            document.Close();
            return await Task.FromResult(memoryStream.ToArray());
        }
    }
}
