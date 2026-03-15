using FreelanceManager.Core.Models;

namespace FreelanceManager.Core.interfaces
{
    public interface IProjectService
    {
        decimal CalculateHourlyTotal(Project project, List<TimeEntry> timeEntries);
    };
}