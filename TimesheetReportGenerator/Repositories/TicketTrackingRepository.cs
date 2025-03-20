using TimesheetReportGenerator.Models;
using TimesheetReportGenerator.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TimesheetReportGenerator.Repositories;

public class TicketTrackingRepository(ApplicationDbContext dbContext) : RepositoryBase(dbContext), ITicketTrackingRepository
{
    public void AddTicketTracking(TicketTracking ticket)
    {
        _dbContext.TicketTrackings.Add(ticket);
    }

    public async Task<List<TicketTracking>> GetAllTicketTracking()
    {
        return await _dbContext.TicketTrackings.ToListAsync();
    }
}