using TimesheetReportGenerator.Models;

namespace TimesheetReportGenerator.Repositories.Interfaces;

public interface ITicketTrackingRepository : IRepositoryBase
{
    public void AddTicketTracking(TicketTracking ticket);
    public Task<List<TicketTracking>> GetAllTicketTracking();
}