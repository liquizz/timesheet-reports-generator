using TimesheetReportGenerator.Models;
using TimesheetReportGenerator.Repositories.Interfaces;
using TimesheetReportGenerator.Services.Interfaces;
using TimesheetReportGenerator.ViewModels;

namespace TimesheetReportGenerator.Services;

public class TicketService(ITicketTrackingRepository repository) : ITicketService
{
    public async Task<bool> AddTicketTracking(TicketViewModel ticket)
    {
        var ticketTracking = new TicketTracking
        {
            TicketId = ticket.TicketId,
            TicketDescription = ticket.TicketDescription,
            TimeSpent = float.Parse(ticket.TimeSpent),
            TicketDate = DateOnly.Parse(ticket.TicketDate)
        };
        
        repository.AddTicketTracking(ticketTracking);
        await repository.SaveChangesAsync();
        
        return true;
    }

    public async Task<List<TicketViewModel>> GetAllTicketTracking()
    {
        var ticketTrackings = await repository.GetAllTicketTracking();
        
        return ticketTrackings.Select(t => new TicketViewModel
        {
            TicketId = t.TicketId,
            TicketDescription = t.TicketDescription,
            TimeSpent = t.TimeSpent.ToString(),
            TicketDate = t.TicketDate.ToString()
        }).ToList();
    }
}