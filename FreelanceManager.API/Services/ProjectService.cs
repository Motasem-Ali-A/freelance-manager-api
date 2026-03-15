using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;

namespace FreelanceManager.API.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public decimal CalculateHourlyTotal(Project project, List<TimeEntry> timeEntries)
        {
            return timeEntries.Sum(t => t.HoursWorked) * project.HourlyRate;
        }
    }
}