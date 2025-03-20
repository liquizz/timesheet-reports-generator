using TimesheetReportGenerator.ViewModels;

namespace TimesheetReportGenerator.Services.Interfaces;

public interface IExcelService
{
    byte[] GenerateReport(List<ReportInput> reportInputs, List<AzureDevOpsTicket> devOpsTickets);
}