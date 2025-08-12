using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Models;

namespace UXComex.GerenciadorPedidos.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> SearchAsync(string searchTerm);
        Task<Product> GetByIdAsync(int id);
        Task<int> AddAsync(Product client);
        Task UpdateAsync(Product client);
        Task DeleteAsync(int id);
        Task<PagedResult<Product>> SearchAndPaginateAsync(string searchTerm, int pageNumber, int pageSize);
        Task UpdateStockAsync(int productId, int quantity);
    }
}