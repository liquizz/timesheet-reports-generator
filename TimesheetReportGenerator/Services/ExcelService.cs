using System.Globalization;
using System.Reflection;
using TimesheetReportGenerator.Services.Interfaces;
using TimesheetReportGenerator.ViewModels;
using ClosedXML.Excel;

namespace TimesheetReportGenerator.Services;

public class ExcelService : IExcelService
{
    public byte[] GenerateReport(List<ReportInput> reportInputs, List<AzureDevOpsTicket> devOpsTickets)
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string resourceName = "TimesheetReportGenerator.Templates.TimesheetTemplate.xlsx"; 

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
                throw new FileNotFoundException("Resource not found: " + resourceName);

            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheets.Worksheet("February 2024");
                
                worksheet.Cell("B5").Value = "Date";
                worksheet.Cell("C5").Value = "Task Description";
                worksheet.Cell("D5").Value = "Project";
                worksheet.Cell("E5").Value = "Effort Spent";
                worksheet.Cell("F5").Value = "Ticket ID with Link";

                int row = 6;

                foreach (var input in reportInputs)
                {
                    foreach (var ticket in input.Tickets)
                    {
                        var devOpsTicket = devOpsTickets.FirstOrDefault(t => t.TicketId == ticket.TicketId);

                        worksheet.Cell(row, 2).Value = input.Date;
                        worksheet.Cell(row, 3).Value = $"{devOpsTicket.TicketId} - {devOpsTicket.TicketName}";
                        // There should be a project name here, you may need to add a new field to the TicketViewModel, or get it from somewhere else
                        worksheet.Cell(row, 4).Value = string.Empty;
                        worksheet.Cell(row, 5).Style.NumberFormat.Format = "0.00";
                        worksheet.Cell(row, 5).Value = double.Parse(ticket.TimeSpent);
                        worksheet.Cell(row, 6).Value = devOpsTicket.TicketLink;
                        worksheet.Cell(row, 6).SetHyperlink(new XLHyperlink(devOpsTicket.TicketLink));

                        row++;
                    }
                }

                var currentMonthYear = DateTime.Now.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
                worksheet.Name = currentMonthYear;
                
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}