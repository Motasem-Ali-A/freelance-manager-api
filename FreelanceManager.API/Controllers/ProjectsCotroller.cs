using System.Security.Claims;
using FreelanceManager.Core.DTOs;
using FreelanceManager.Core.DTOs.Project;
using FreelanceManager.Core.Enums;
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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectRepository projectRepository, IProjectService projectService)
        {
            _projectRepository = projectRepository;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects([FromQuery] string? billingType,
             [FromQuery] string? status, [FromQuery] int? clientId,
             [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var projects = await _projectRepository.GetAllByUserIdAsync(userId, clientId, status, billingType, page, pageSize);
            var responses = new List<ProjectResponseDto>();
            foreach (var project in projects.Data)
            {
                responses.Add(new ProjectResponseDto
                {
                    Id = project.Id,
                    CreatedAt = project.CreatedAt,
                    Title = project.Title,
                    Description = project.Description,
                    Status = project.Status.ToString(),
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    HourlyRate = project.HourlyRate,
                    FixedPrice = project.FixedPrice,
                    BillingType = project.BillingType.ToString(),
                    ClientId = project.ClientId
                });
            }
            return Ok(new PageResult<ProjectResponseDto>
            {
                Data = responses,
                TotalCount = projects.TotalCount,
                Page = projects.Page,
                PageSize = projects.PageSize,
                TotalPages = projects.TotalPages
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectRepository.GetProjectWithTimeEntriesAsync(id);
            if (project == null)
                throw new NotFoundException($"Project with ID {id} not found");

            return Ok(new ProjectResponseDto
            {
                Id = project.Id,
                CreatedAt = project.CreatedAt,
                Title = project.Title,
                Description = project.Description,
                Status = project.Status.ToString(),
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                HourlyRate = project.HourlyRate,
                HourlyTotal = _projectService.CalculateHourlyTotal(project, project.TimeEntries.ToList()),
                FixedPrice = project.FixedPrice,
                BillingType = project.BillingType.ToString(),
                ClientId = project.ClientId
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] CreateProjectDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var project = new Project
            {
                CreatedAt = DateTime.UtcNow,
                Title = dto.Title,
                Description = dto.Description,
                Status = ProjectStatus.NotStarted,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                HourlyRate = dto.HourlyRate,
                FixedPrice = dto.FixedPrice,
                BillingType = Enum.Parse<BillingType>(dto.BillingType),
                ClientId = dto.ClientId,
                UserId = userId
            };
            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();

            var response = new ProjectResponseDto
            {
                Id = project.Id,
                CreatedAt = project.CreatedAt,
                Title = project.Title,
                Description = project.Description,
                Status = project.Status.ToString(),
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                HourlyRate = project.HourlyRate,
                HourlyTotal = 0,
                FixedPrice = project.FixedPrice,
                BillingType = project.BillingType.ToString(),
                ClientId = project.ClientId
                
            };

            return CreatedAtAction(nameof(GetById), new { id = project.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectDto dto)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new NotFoundException($"Project with ID {id} not found");
            project.Title = dto.Title;
            project.Description = dto.Description;
            project.Status = Enum.Parse<ProjectStatus>(dto.Status);
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
                Status = project.Status.ToString(),
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                HourlyRate = project.HourlyRate,
                HourlyTotal = 0,
                FixedPrice = project.FixedPrice,
                BillingType = project.BillingType.ToString(),
                ClientId = project.ClientId
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new NotFoundException($"Project with ID {id} not found");
            _projectRepository.Delete(project);
            await _projectRepository.SaveChangesAsync();
            return NoContent();
        }

    }

}