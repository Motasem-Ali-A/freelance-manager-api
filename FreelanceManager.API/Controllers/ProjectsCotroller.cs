using FreelanceManager.Core.DTOs.Project;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private static List<ProjectResponseDto> _projects = new List<ProjectResponseDto>
        {
            new ProjectResponseDto
            {
                Id =1,
                CreatedAt = DateTime.UtcNow,
                Title = "E-commerce Website",
                Description ="build an E-commerce Website",
                Status = "Not started",
                StartDate = DateTime.Today,
                EndDate = new DateTime (2026, 6, 30),
                HourlyRate = 20,
                FixedPrice = 0,
                BillingType = "Hourly",
                ClientId =1
            }
            ,
            new ProjectResponseDto
            {
                Id =2,
                CreatedAt = DateTime.UtcNow,
                Title = "Website Redesign",
                Description ="modren website Redesign",
                Status = "in progress",
                StartDate = DateTime.Today,
                EndDate = new DateTime (2026, 4, 15),
                HourlyRate = 0,
                FixedPrice = 300,
                BillingType = "Fixed",
                ClientId =2

            }
        };

        [HttpGet]
        public IActionResult GetAllProjects()
        {
            return Ok(_projects);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
                return NotFound($"Project with ID {id} not found");
            return Ok(project);
        }
        [HttpPost]
        public IActionResult AddProject([FromBody] CreateProjectDto dto)
        {
            var project = new ProjectResponseDto
            {
                Id = _projects.Count + 1,
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
            _projects.Add(project);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProject(int id, [FromBody] UpdateProjectDto dto)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
                return NotFound($"Project with ID {id} not found");

            project.Title = dto.Title;
            project.Description = dto.Description;
            project.Status = dto.Status;
            project.EndDate = dto.EndDate;
            project.HourlyRate = dto.HourlyRate;
            project.FixedPrice = dto.FixedPrice;
            return Ok(project);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
                return NotFound($"Project with ID {id} not found");
            _projects.Remove(project);
            return NoContent();
        }

    }

}