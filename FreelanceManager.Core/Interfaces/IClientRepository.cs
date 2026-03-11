using FreelanceManager.Core.Models;
namespace FreelanceManager.Core.interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client?> GetClientWithProjectsAsync(int id);
    }
}