using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Models;

namespace UXComex.GerenciadorPedidos.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for the business logic related to the Client entity.
    /// </summary>
    public interface IClientService
    {
        Task<int> CreateAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(int id);
        Task<Client> GetByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllAsync();
        Task<PagedResult<Client>> SearchAsync(string searchTerm, int pageNumber, int pageSize);
    }
}