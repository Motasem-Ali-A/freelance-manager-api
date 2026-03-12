using FreelanceManager.Core.DTOs.TimeEntry;
using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        public TimeEntriesController(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var timeEntries = await _timeEntryRepository.GetAllAsync();
            var responses = new List<TimeEntryResponseDto>();
            foreach (var timeEntry in timeEntries)
            {
                responses.Add(new TimeEntryResponseDto
                {
                    Id = timeEntry.Id,
                    CreatedAt = timeEntry.CreatedAt,
                    Date = timeEntry.Date,
                    HoursWorked = timeEntry.HoursWorked,
                    Description = timeEntry.Description,
                    ProjectId = timeEntry.ProjectId

                });
            }
            return Ok(responses);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
            if (timeEntry == null)
                return NotFound($"Time Entry with ID {id} Not Found");
            return Ok(new TimeEntryResponseDto
            {
                Id = timeEntry.Id,
                CreatedAt = timeEntry.CreatedAt,
                Date = timeEntry.Date,
                HoursWorked = timeEntry.HoursWorked,
                Description = timeEntry.Description,
                ProjectId = timeEntry.ProjectId
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddTimeEntry([FromBody] CreateTimeEntryDto dto)
        {
            var timeEntry = new TimeEntry
            {
                CreatedAt = DateTime.UtcNow,
                Date = dto.Date,
                HoursWorked = dto.HoursWorked,
                Description = dto.Description,
                ProjectId = dto.ProjectId
            };
            await _timeEntryRepository.AddAsync(timeEntry);
            await _timeEntryRepository.SaveChangesAsync();
            var response = new TimeEntryResponseDto
            {
                Id = timeEntry.Id,
                CreatedAt = timeEntry.CreatedAt,
                Date = timeEntry.Date,
                HoursWorked = timeEntry.HoursWorked,
                Description = timeEntry.Description,
                ProjectId = timeEntry.ProjectId
            };
            return CreatedAtAction(nameof(GetById), new { id = timeEntry.Id }, response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimeEntry(int id, [FromBody] UpdateTimeEntryDto dto)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
            if (timeEntry == null)
                return NotFound($"Time Entry with ID {id} Not Found");
            timeEntry.HoursWorked = dto.HoursWorked;
            timeEntry.Description = dto.Description;
            _timeEntryRepository.Update(timeEntry);
            await _timeEntryRepository.SaveChangesAsync();
            return Ok(new TimeEntryResponseDto
            {
                Id = timeEntry.Id,
                CreatedAt = timeEntry.CreatedAt,
                Date = timeEntry.Date,
                HoursWorked = timeEntry.HoursWorked,
                Description = timeEntry.Description,
                ProjectId = timeEntry.ProjectId
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeEntry(int id)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
            if (timeEntry == null)
                return NotFound($"Time Entry with ID {id} Not Found");
            _timeEntryRepository.Delete(timeEntry);
            await _timeEntryRepository.SaveChangesAsync();
            return NoContent();
        }

    }
}

