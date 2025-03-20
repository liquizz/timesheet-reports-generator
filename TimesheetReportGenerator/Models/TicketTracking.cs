using System.ComponentModel.DataAnnotations;

namespace TimesheetReportGenerator.Models;

public class TicketTracking
{
    [Key]
    public int Id { get; set; }

    public string TicketId { get; set; }
    public string TicketDescription { get; set; }
    public float TimeSpent { get; set; }
    public DateOnly TicketDate { get; set; }
}