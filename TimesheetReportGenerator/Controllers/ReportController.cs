using System.Globalization;
using TimesheetReportGenerator.Services.Interfaces;
using TimesheetReportGenerator.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace TimesheetReportGenerator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController(
    IAzureDevOpsService azureDevOpsService, 
    IExcelService excelService, 
    ITicketService ticketService) : ControllerBase
{
    /// <summary>
    /// Generates a report based on the provided inputs
    /// </summary>
    /// <param name="reportInputs"></param>
    /// <returns></returns>
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateReport([FromBody] List<ReportInput> reportInputs)
    {
        var devOpsTickets = new List<AzureDevOpsTicket>();

        var tickets = reportInputs.SelectMany(input => input.Tickets).ToList();
        
        devOpsTickets = await azureDevOpsService.GetManyTicketDetailsAsync(tickets.Select(t => t.TicketId).ToList());
        
        var report = excelService.GenerateReport(reportInputs, devOpsTickets);
        
        var currentMonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
        
        return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Timesheet {currentMonth}.xlsx");
    }

    [HttpGet("generate")]
    public async Task<IActionResult> GenerateReport()
    {
        var devOpsTickets = new List<AzureDevOpsTicket>();

        var ticketsData = await ticketService.GetAllTicketTracking();
        
        var reportInputs = ticketsData
            .GroupBy(t => t.TicketDate)
            .Select(g => new ReportInput
            {
                Date = g.Key,
                Tickets = g.Select(t => new TicketInfo
                {
                    TicketId = t.TicketId,
                    TimeSpent = t.TimeSpent.ToString()
                }).ToList()
            }).ToList();

        var tickets = reportInputs.SelectMany(input => input.Tickets).ToList();
        
        devOpsTickets = await azureDevOpsService.GetManyTicketDetailsAsync(tickets.Select(t => t.TicketId).ToList());
        
        var report = excelService.GenerateReport(reportInputs, devOpsTickets);
        
        var currentMonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
        
        return File(report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Timesheet {currentMonth}.xlsx");
    }
    
    [HttpGet("report-inputs")]
    public async Task<IActionResult> GetReportInputs()
    {
        var ticketsData = await ticketService.GetAllTicketTracking();
        
        var reportInputs = ticketsData
            .GroupBy(t => t.TicketDate)
            .Select(g => new ReportInput
            {
                Date = g.Key,
                Tickets = g.Select(t => new TicketInfo
                {
                    TicketId = t.TicketId,
                    TimeSpent = t.TimeSpent.ToString()
                }).ToList()
            }).ToList();
        
        return Ok(reportInputs);
    }
    
    /// <summary>
    /// Compares the time spent on tickets with the current sprint
    /// </summary>
    /// <param name="reportInputs"></param>
    /// <returns>Difference in ticketId and TimeSpent (if time is negative difference on Azure Devops board,
    /// if positive the difference in the report inputs)</returns>
    [HttpPost("compare-time")]
    public async Task<IActionResult> CompareTimeWithCurrentSprint([FromBody] List<ReportInput> reportInputs)
    {
        var tickets = reportInputs.SelectMany(input => input.Tickets).ToList();
        
        var aggregatedTickets = tickets.GroupBy(t => t.TicketId).Select(g => new 
        {
            TicketId = g.Key,
            TimeSpent = g.Sum(t => double.Parse(t.TimeSpent)).ToString()
        }).ToList();

        var currentTickets = await azureDevOpsService.GetWorkItemsForCurrentSprintAsync();
        
        var difference = new List<TicketInfo>();
        
        foreach (var ticket in aggregatedTickets)
        {
            var currentTicket = currentTickets.FirstOrDefault(t => t.TicketId == ticket.TicketId);
            
            if (currentTicket != null)
            {
                var timeSpent = string.IsNullOrEmpty(ticket.TimeSpent) ? 0 : double.Parse(ticket.TimeSpent);
                var currentTimeSpent = string.IsNullOrEmpty(currentTicket.CompletedWork) ? 0 : double.Parse(currentTicket.CompletedWork);
                
                if (timeSpent != currentTimeSpent)
                {
                    var timeDifference = timeSpent - currentTimeSpent;
                    
                    difference.Add(new TicketInfo
                    {
                        TicketId = ticket.TicketId,
                        TimeSpent = timeDifference.ToString()
                    });
                }
            }
        }
        
        difference.AddRange(currentTickets.Where(t => aggregatedTickets.All(a => a.TicketId != t.TicketId)).Select(t => new TicketInfo
        {
            TicketId = t.TicketId,
            TimeSpent = $"-{t.CompletedWork}"
        }));
        
        return Ok(difference);
    }
    
    /// <summary>
    /// Returns the difference in tickets between the report inputs and the current sprint
    /// </summary>
    /// <param name="reportInputs"></param>
    /// <returns></returns>
    [HttpPost("compare-tickets")]
    public async Task<IActionResult> CompareTicketsWithCurrentSprint([FromBody] List<ReportInput> reportInputs)
    {
        var tickets = reportInputs.SelectMany(input => input.Tickets).ToList();
        
        var aggregatedTickets = tickets.GroupBy(t => t.TicketId).Select(g => new 
        {
            TicketId = g.Key,
            TimeSpent = g.Sum(t => double.Parse(t.TimeSpent)).ToString()
        }).ToList();

        var currentTickets = await azureDevOpsService.GetWorkItemsForCurrentSprintAsync();
        
        var differenceInTickets = currentTickets.Where(t => aggregatedTickets.All(a => a.TicketId != t.TicketId)).ToList();
        
        return Ok(differenceInTickets);
    }
}
