using TimesheetReportGenerator.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TimesheetReportGenerator.Controllers;

[ApiController]
[Route("api/[controller]/tickets")]
public class AzureDevOpsDataController(IAzureDevOpsService azureDevOpsService) : ControllerBase
{
    /// <summary>
    /// Gets a list of tickets for the current sprint
    /// </summary>
    /// <returns></returns>
    [HttpGet("current-sprint")]
    public async Task<IActionResult> GetTicketsForCurrentMonth()
    {
        var tickets = await azureDevOpsService.GetWorkItemsForCurrentSprintAsync();

        return Ok(tickets);
    }
    
    /// <summary>
    /// Gets completed work for the current sprint
    /// </summary>
    /// <returns></returns>
    [HttpGet("completed-work")]
    public async Task<IActionResult> GetCompletedWorkForCurrentSprint()
    {
        var tickets = await azureDevOpsService.GetWorkItemsForCurrentSprintAsync();

        var completedWork = tickets.Sum(t => string.IsNullOrEmpty(t.CompletedWork) ? 0 : double.Parse(t.CompletedWork));
        
        return Ok(completedWork);
    }
}