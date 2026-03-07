using System.Xml;
using FreelanceManager.Core.DTOs.Client;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private static List<ClientResponseDto> _clients = new List<ClientResponseDto>
        {
            new ClientResponseDto
            {
                Id =1,
                Name = "first name1",
                Email= "example1@domain.com",
                Phone ="123456789",
                CompanyName = "example company 1",
                Address = "123 Main st",
                Notes = "Important Client",
                Status = "Active",
                CreatedAt = DateTime.UtcNow

            },
            new ClientResponseDto
            {
                Id =2,
                Name = "first name2",
                Email= "example2@domain.com",
                Phone ="111213141",
                CompanyName = "example company 2",
                Address = "456 Main st",
                Notes = "",
                Status = "Active",
                CreatedAt = DateTime.UtcNow

            }
        };

        [HttpGet]
        public IActionResult GetAllClients()
        {
            return Ok(_clients);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
                return NotFound($"Client with ID {id} not found");
            return Ok(client);
        }
        [HttpPost]
        public IActionResult AddClient([FromBody] CreateClientDto dto)
        {
            var client = new ClientResponseDto
            {
                Id = _clients.Count + 1,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                CompanyName = dto.CompanyName,
                Address = dto.Address,
                Notes = dto.Notes

            };
            _clients.Add(client);
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, [FromBody] UpdateClientDto dto)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
                return NotFound($"Client with ID {id} not found");
            client.Name = dto.Name;
            client.Address = dto.Address;
            client.CompanyName = dto.CompanyName;
            client.Notes = dto.Notes;
            client.Phone = dto.Phone;
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
                return NotFound($"Client with ID {id} not found");
            _clients.Remove(client);
            return NoContent();
        }

    }

}