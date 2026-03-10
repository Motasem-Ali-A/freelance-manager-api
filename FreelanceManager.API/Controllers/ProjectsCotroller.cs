using FreelanceManager.Core.DTOs.Project;
using FreelanceManager.Core.Models;
using FreelanceManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _context.Projects.ToListAsync();
            var responses = new List<ProjectResponseDto>();
            foreach (var project in projects)
            {
                responses.Add(new ProjectResponseDto
                {
                    Id = project.Id,
                    CreatedAt = project.CreatedAt,
                    Title = project.Title,
                    Description = project.Description,
                    Status = project.Status,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    HourlyRate = project.HourlyRate,
                    FixedPrice = project.FixedPrice,
                    BillingType = project.BillingType,
                    ClientId = project.ClientId
                });
            }
            return Ok(responses);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
                return NotFound($"Project with ID {id} not found");
            return Ok(new ProjectResponseDto
            {
                Id = project.Id,
                CreatedAt = project.CreatedAt,
                Title = project.Title,
                Description = project.Description,
                Status = project.Status,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                HourlyRate = project.HourlyRate,
                FixedPrice = project.FixedPrice,
                BillingType = project.BillingType,
                ClientId = project.ClientId
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] CreateProjectDto dto)
        {
            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = "Not started",
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                HourlyRate = dto.HourlyRate,
                FixedPrice = dto.FixedPrice,
                BillingType = dto.BillingType,
                ClientId = dto.ClientId
            };
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            var response = new ProjectResponseDto
            {
                Id = project.Id,
                CreatedAt = project.CreatedAt,
                Title = project.Title,
                Description = project.Description,
                Status = project.Status,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                HourlyRate = project.HourlyRate,
                FixedPrice = project.FixedPrice,
                BillingType = project.BillingType,
                ClientId = project.ClientId
            };

            return CreatedAtAction(nameof(GetById), new { id = project.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectDto dto)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
                return NotFound($"Project with ID {id} not found");
            project.Title = dto.Title;
            project.Description = dto.Description;
            project.Status = dto.Status;
            project.EndDate = dto.EndDate;
            project.HourlyRate = dto.HourlyRate;
            project.FixedPrice = dto.FixedPrice;

            await _context.SaveChangesAsync();
            return Ok(new ProjectResponseDto
            {
                Id = project.Id,
                CreatedAt = project.CreatedAt,
                Title = project.Title,
                Description = project.Description,
                Status = project.Status,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                HourlyRate = project.HourlyRate,
                FixedPrice = project.FixedPrice,
                BillingType = project.BillingType,
                ClientId = project.ClientId
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
                return NotFound($"Project with ID {id} not found");
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }

}