using TimesheetReportGenerator.Services.Interfaces;
using TimesheetReportGenerator.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace TimesheetReportGenerator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketTrackingController(ITicketService service) : ControllerBase
{
    /// <summary>
    /// Adds a new ticket tracking entry.
    /// </summary>
    /// <param name="ticket">The ticket tracking information to add.</param>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpPost("add")]
    public async Task<IActionResult> AddTicketTracking([FromBody] TicketViewModel ticket)
    {
        await service.AddTicketTracking(ticket);
    
        return Ok();
    }

    /// <summary>
    /// Adds multiple ticket tracking entries in bulk.
    /// </summary>
    /// <param name="ticket">A list of ticket tracking information to add.</param>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpPost("add-bulk")]
    public async Task<IActionResult> AddBulkTicketTracking([FromBody] List<TicketViewModel> ticket)
    {
        foreach (var t in ticket)
        {
            await service.AddTicketTracking(t);
        }
    
        return Ok();
    }

    /// <summary>
    /// Retrieves all ticket tracking entries.
    /// </summary>
    /// <returns>An IActionResult containing a list of all ticket tracking entries.</returns>
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllTicketTracking()
    {
        var tickets = await service.GetAllTicketTracking();
    
        return Ok(tickets);
    }
}