using FreelanceManager.Core.DTOs.Client;
using FreelanceManager.Data;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ClientsController(ApplicationDbContext context)
        {
            _context = context;

        }

        /*[HttpGet]
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
        }*/

    }

}