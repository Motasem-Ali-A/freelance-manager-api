using FreelanceManager.Core.DTOs.Client;
using FreelanceManager.Core.Models;
using FreelanceManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _context.Clients.ToListAsync();
            var responses = new List<ClientResponseDto>();

            foreach (var client in clients)
            {
                responses.Add(new ClientResponseDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email,
                    Phone = client.Phone,
                    CompanyName = client.CompanyName,
                    Address = client.Address,
                    Notes = client.Notes,
                    Status = client.Status,
                    CreatedAt = client.CreatedAt
                });
            }

            return Ok(responses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
                return NotFound($"Client with ID {id} not found");
            return Ok(new ClientResponseDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                CompanyName = client.CompanyName,
                Address = client.Address,
                Notes = client.Notes,
                Status = client.Status,
                CreatedAt = client.CreatedAt
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] CreateClientDto dto)
        {
            var client = new Client
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                CompanyName = dto.CompanyName,
                Address = dto.Address,
                Notes = dto.Notes,
                Status = "Active",
                CreatedAt = DateTime.UtcNow

            };
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            var response = new ClientResponseDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                CompanyName = client.CompanyName,
                Address = client.Address,
                Notes = client.Notes,
                Status = client.Status,
                CreatedAt = client.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = client.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientDto dto)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
                return NotFound($"Client with ID {id} not found");
            client.Name = dto.Name;
            client.Address = dto.Address;
            client.CompanyName = dto.CompanyName;
            client.Notes = dto.Notes;
            client.Phone = dto.Phone;
            await _context.SaveChangesAsync();

            return Ok(new ClientResponseDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                CompanyName = client.CompanyName,
                Address = client.Address,
                Notes = client.Notes,
                Status = client.Status,
                CreatedAt = client.CreatedAt
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
                return NotFound($"Client with ID {id} not found");
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }

}