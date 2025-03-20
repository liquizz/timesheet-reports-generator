using TimesheetReportGenerator.ViewModels;

namespace TimesheetReportGenerator.Services.Interfaces;

public interface IAzureDevOpsService
{
    Task<AzureDevOpsTicket> GetTicketDetailsAsync(string ticketId);
    Task<List<AzureDevOpsTicket>> GetManyTicketDetailsAsync(List<string> ticketIds);
    Task<List<AzureDevOpsTicket>> GetWorkItemsForCurrentSprintAsync();
}