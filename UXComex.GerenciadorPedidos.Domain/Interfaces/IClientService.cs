using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Interfaces;
using UXComex.GerenciadorPedidos.Domain.Models;

namespace UXComex.GerenciadorPedidos.Domain.Services
{
    /// <summary>
    /// Defines the contract for the business logic related to the Client entity.
    /// </summary>
    public interface IClientService
    {
        Task<int> CreateClientAsync(Client client);
        Task UpdateClientAsync(Client client);
        Task DeleteClientAsync(int id);
        Task<Client> GetClientByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<PagedResult<Client>> SearchClientsAsync(string searchTerm, int pageNumber, int pageSize);
    }
}