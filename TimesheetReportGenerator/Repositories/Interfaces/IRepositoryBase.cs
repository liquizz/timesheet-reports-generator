namespace TimesheetReportGenerator.Repositories.Interfaces;

public interface IRepositoryBase
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}