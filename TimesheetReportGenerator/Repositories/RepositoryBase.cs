using TimesheetReportGenerator.Models;

namespace TimesheetReportGenerator.Repositories;

public abstract class RepositoryBase : IDisposable
{
    protected ApplicationDbContext _dbContext;

    protected RepositoryBase(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}