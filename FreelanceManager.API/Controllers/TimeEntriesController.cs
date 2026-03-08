using FreelanceManager.Core.DTOs.TimeEntry;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeEntriesController : ControllerBase
    {
        private static List<TimeEntryResponseDto> _timeEntries = new List<TimeEntryResponseDto>
        {
            new TimeEntryResponseDto
            {
                Id =1,
                CreatedAt = DateTime.UtcNow,
                Date = new DateTime(2026, 04, 05),
                HoursWorked = 5,
                Description = "Login Page Done",
                ProjectId = 1
            },
            new TimeEntryResponseDto
            {
                Id =2,
                CreatedAt = DateTime.UtcNow,
                Date = new DateTime(2026, 04, 06),
                HoursWorked = 3,
                Description = "Navigation bar Done",
                ProjectId = 1
            }

        };
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_timeEntries);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var timeEntry = _timeEntries.FirstOrDefault(t => t.Id == id);
            if (timeEntry == null)
                return NotFound($"Time Entry with ID {id} Not Found");
            return Ok(timeEntry);
        }
        [HttpPost]
        public IActionResult AddTimeEntry([FromBody] CreateTimeEntryDto dto)
        {
            var timeEntry = new TimeEntryResponseDto
            {
                Id = _timeEntries.Count + 1,
                CreatedAt = DateTime.UtcNow,
                Date = dto.Date,
                HoursWorked = dto.HoursWorked,
                Description = dto.Description,
                ProjectId = dto.ProjectId
            };
            _timeEntries.Add(timeEntry);
            return CreatedAtAction(nameof(GetById), new { id = timeEntry.Id }, timeEntry);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateTimeEntry(int id, [FromBody] UpdateTimeEntryDto dto)
        {
            var timeEntry = _timeEntries.FirstOrDefault(t => t.Id == id);
            if (timeEntry == null)
                return NotFound($"Time Entry with ID {id} Not Found");
            timeEntry.HoursWorked = dto.HoursWorked;
            timeEntry.Description = dto.Description;
            return Ok(timeEntry);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteTimeEntry(int id)
        {
            var timeEntry = _timeEntries.FirstOrDefault(t => t.Id == id);
            if (timeEntry == null)
                return NotFound($"Time Entry with ID {id} Not Found");
            _timeEntries.Remove(timeEntry);
            return NoContent();
        }

    }
}

