using System.Security.Claims;
using FreelanceManager.Core.DTOs.TimeEntry;
using FreelanceManager.Core.Exceptions;
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

        /// <summary>
        /// Get all time entries for the authenticated user
        /// </summary>
        /// <returns>A list of time entries</returns>
        [HttpGet]
        [ProducesResponseType(typeof(TimeEntryResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var timeEntries = await _timeEntryRepository.GetAllByUserIdAsync(userId);
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

        /// <summary>
        /// Get a time entry by ID
        /// </summary>
        /// <param name="id">The time entry ID</param>
        /// <returns>Time entry details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TimeEntryResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
            if (timeEntry == null)
                throw new NotFoundException($"Time Entry with ID {id} Not Found");
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

        /// <summary>
        /// Add a time entry
        [HttpPost]
        [ProducesResponseType(typeof(TimeEntryResponseDto), 201)]
        public async Task<IActionResult> AddTimeEntry([FromBody] CreateTimeEntryDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var timeEntry = new TimeEntry
            {
                CreatedAt = DateTime.UtcNow,
                Date = dto.Date,
                HoursWorked = dto.HoursWorked,
                Description = dto.Description,
                ProjectId = dto.ProjectId,
                UserId = userId
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

         /// <summary>
        /// Update the information of certian time entry
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TimeEntryResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTimeEntry(int id, [FromBody] UpdateTimeEntryDto dto)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
            if (timeEntry == null)
                throw new NotFoundException($"Time Entry with ID {id} Not Found");
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

         /// <summary>
        /// Delete a time entry
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTimeEntry(int id)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
            if (timeEntry == null)
                throw new NotFoundException($"Time Entry with ID {id} Not Found");
            _timeEntryRepository.Delete(timeEntry);
            await _timeEntryRepository.SaveChangesAsync();
            return NoContent();
        }

    }
}

