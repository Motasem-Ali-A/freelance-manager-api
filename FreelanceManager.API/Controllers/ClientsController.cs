using System.Security.Claims;
using FreelanceManager.Core.DTOs;
using FreelanceManager.Core.DTOs.Client;
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
    [Tags("Clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;

        }
        /// <summary>
        /// Get all clients for the authenticated user
        /// </summary>
        /// <param name="status">Filter by status (Active/Inactive)</param>
        /// <param name="search">Search by name or company name</param>
        /// <returns>A paginated list of clients</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ClientResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllClients([FromQuery] string? status,
            [FromQuery] string? search, [FromQuery] int page, [FromQuery] int pagesize)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var clients = await _clientRepository.GetAllByUserIdAsync(userId, status, search, page, pagesize);
            var responses = new List<ClientResponseDto>();

            foreach (var client in clients.Data)
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
                    CreatedAt = client.CreatedAt,
                });
            }

            return Ok(new PageResult<ClientResponseDto>
            {
                Data = responses,
                TotalCount = clients.TotalCount,
                Page = clients.Page,
                PageSize = clients.PageSize,
                TotalPages = clients.TotalPages
            });
        }

        /// <summary>
        /// Get a client by ID
        /// </summary>
        /// <param name="id">The client ID</param>
        /// <returns>Client details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClientResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                throw new NotFoundException($"Client with ID {id} not found");
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

        /// <summary>
        /// Add a client
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ClientResponseDto), 201)]
        public async Task<IActionResult> AddClient([FromBody] CreateClientDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var client = new Client
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                CompanyName = dto.CompanyName,
                Address = dto.Address,
                Notes = dto.Notes,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UserId = userId

            };
            await _clientRepository.AddAsync(client);
            await _clientRepository.SaveChangesAsync();

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

        /// <summary>
        /// Update the information of certian client
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ClientResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientDto dto)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                throw new NotFoundException($"Client with ID {id} not found");
            client.Name = dto.Name;
            client.Address = dto.Address;
            client.CompanyName = dto.CompanyName;
            client.Notes = dto.Notes;
            client.Phone = dto.Phone;
            _clientRepository.Update(client);
            await _clientRepository.SaveChangesAsync();

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

        /// <summary>
        /// Delete a client and all their associated projects and invoices
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                throw new NotFoundException($"Client with ID {id} not found");
            _clientRepository.Delete(client);
            await _clientRepository.SaveChangesAsync();
            return NoContent();
        }

    }

}