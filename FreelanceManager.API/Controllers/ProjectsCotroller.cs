using FreelanceManager.Core.DTOs.Project;
using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectRepository.GetAllAsync();
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
            var project = await _projectRepository.GetByIdAsync(id);
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
                CreatedAt = DateTime.UtcNow,
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
            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();

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
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                return NotFound($"Project with ID {id} not found");
            project.Title = dto.Title;
            project.Description = dto.Description;
            project.Status = dto.Status;
            project.EndDate = dto.EndDate;
            project.HourlyRate = dto.HourlyRate;
            project.FixedPrice = dto.FixedPrice;

            _projectRepository.Update(project);
            await _projectRepository.SaveChangesAsync();
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
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                return NotFound($"Project with ID {id} not found");
            _projectRepository.Delete(project);
            await _projectRepository.SaveChangesAsync();
            return NoContent();
        }

    }

}