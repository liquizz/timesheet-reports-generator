namespace TimesheetReportGenerator.ViewModels;

public class ReportInput
{
    public string? Date { get; set; }
    public List<TicketInfo> Tickets { get; set; }
}

public class TicketInfo
{
    public string? TicketId { get; set; }
    public string? TimeSpent { get; set; }
}

public class AzureDevOpsTicket
{
    public string? TicketId { get; set; }
    public string? TicketName { get; set; }
    public string? TicketLink { get; set; }
    public string? CompletedWork { get; set; }
}