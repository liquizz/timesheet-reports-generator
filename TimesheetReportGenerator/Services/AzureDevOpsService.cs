using TimesheetReportGenerator.Services.Interfaces;
using TimesheetReportGenerator.ViewModels;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Profile;
using Microsoft.VisualStudio.Services.Profile.Client;
using Microsoft.VisualStudio.Services.WebApi;

namespace TimesheetReportGenerator.Services;

public class AzureDevOpsService(IConfiguration configuration) : IAzureDevOpsService
{
    private readonly string? _organizationUrl = configuration["AzureDevOps:OrganizationUrl"];
    private readonly string? _personalAccessToken = configuration["AzureDevOps:PersonalAccessToken"];
    private readonly string? _projectName = configuration["AzureDevOps:ProjectName"];
    private readonly string? _teamName = configuration["AzureDevOps:Team"];
    
    public async Task<AzureDevOpsTicket> GetTicketDetailsAsync(string ticketId)
    {
        var orgUrl = new Uri(_organizationUrl);
        var credentials = new VssBasicCredential(string.Empty, _personalAccessToken);
        var connection = new VssConnection(orgUrl, credentials);
        
        using var httpClient = connection.GetClient<WorkItemTrackingHttpClient>();
        
        var workItem = await httpClient.GetWorkItemAsync(int.Parse(ticketId));

        // Extract the necessary fields from the work item
        var ticketName = workItem.Fields["System.Title"].ToString();
        
        var ticketLink = $"{_organizationUrl}/_workitems/edit/{ticketId}";

        return new AzureDevOpsTicket
        {
            TicketId = ticketId,
            TicketName = ticketName,
            TicketLink = ticketLink,
            CompletedWork = workItem.Fields.ContainsKey("Microsoft.VSTS.Scheduling.CompletedWork")
                ? workItem.Fields["Microsoft.VSTS.Scheduling.CompletedWork"].ToString()
                : string.Empty
        };
    }

    public async Task<List<AzureDevOpsTicket>> GetManyTicketDetailsAsync(List<string> ticketIds)
    {
        var orgUrl = new Uri(_organizationUrl);
        var credentials = new VssBasicCredential(string.Empty, _personalAccessToken);
        var connection = new VssConnection(orgUrl, credentials);
        
        using var httpClient = connection.GetClient<WorkItemTrackingHttpClient>();
        
        var ids = ticketIds.Select(int.Parse).ToList();
        
        List<WorkItem> workItems = await httpClient.GetWorkItemsAsync(ids);

        return workItems.Select(i => new AzureDevOpsTicket
        {
            TicketId = i.Id.ToString(),
            TicketName = i.Fields["System.Title"].ToString(),
            TicketLink = $"{_organizationUrl}/_workitems/edit/{i.Id}",
            CompletedWork = i.Fields.ContainsKey("Microsoft.VSTS.Scheduling.CompletedWork")
                ? i.Fields["Microsoft.VSTS.Scheduling.CompletedWork"].ToString()
                : string.Empty
        }).ToList();
    }

    public async Task<List<AzureDevOpsTicket>> GetWorkItemsForCurrentSprintAsync()
    {
        var orgUrl = new Uri(_organizationUrl);
        var credentials = new VssBasicCredential(string.Empty, _personalAccessToken);
        var connection = new VssConnection(orgUrl, credentials);
        
        using var profileClient = connection.GetClient<ProfileHttpClient>();
        
        var profile = await profileClient.GetProfileAsync(new ProfileQueryContext(AttributesScope.Core));
        
        using var workClient = connection.GetClient<WorkHttpClient>();
        
        var teamContext = new TeamContext(_projectName, _teamName);
        var iterations = await workClient.GetTeamIterationsAsync(teamContext);
        
        var currentSprint = iterations.LastOrDefault(iteration =>
            iteration.Attributes.TimeFrame == TimeFrame.Current); // You may change it to Past or Future

        if (currentSprint == null)
        {
            throw new InvalidOperationException("No current sprint found.");
        }
        
        using var httpClient = connection.GetClient<WorkItemTrackingHttpClient>();
        
        var wiql = new Wiql
        {
            Query = $@"
            SELECT [System.Id], [System.Title], [System.CreatedDate], [System.AssignedTo], [Microsoft.VSTS.Scheduling.CompletedWork]
            FROM workitems
            WHERE [System.IterationPath] = '{currentSprint.Path}'
            AND [System.AssignedTo] = '{profile.EmailAddress}'
            ORDER BY [System.CreatedDate] DESC"
        };
        
        var result = await httpClient.QueryByWiqlAsync(wiql);
        
        var ids = result.WorkItems.Select(wi => wi.Id).ToArray();
        var workItems = await httpClient.GetWorkItemsAsync(ids);


        return workItems.Select(i => new AzureDevOpsTicket
        {
            TicketId = i.Id.ToString(),
            TicketName = i.Fields["System.Title"].ToString(),
            TicketLink = $"{_organizationUrl}/_workitems/edit/{i.Id}",
            CompletedWork = i.Fields.ContainsKey("Microsoft.VSTS.Scheduling.CompletedWork")
                ? i.Fields["Microsoft.VSTS.Scheduling.CompletedWork"].ToString()
                : string.Empty
        }).ToList();
    }
}
