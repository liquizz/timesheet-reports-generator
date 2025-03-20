using TimesheetReportGenerator.ViewModels;

namespace TimesheetReportGenerator.Services.Interfaces;

public interface ITicketService
{
    public Task<bool> AddTicketTracking(TicketViewModel ticket);
    public Task<List<TicketViewModel>> GetAllTicketTracking();
}