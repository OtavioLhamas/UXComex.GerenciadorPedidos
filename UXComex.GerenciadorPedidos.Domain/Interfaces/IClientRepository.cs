using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Models;

namespace UXComex.GerenciadorPedidos.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<IEnumerable<Client>> SearchAsync(string searchTerm);
        Task<Client> GetByIdAsync(int id);
        Task<int> AddAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(int id);
        Task<PagedResult<Client>> SearchAndPaginateAsync(string searchTerm, int pageNumber, int pageSize);
    }
}