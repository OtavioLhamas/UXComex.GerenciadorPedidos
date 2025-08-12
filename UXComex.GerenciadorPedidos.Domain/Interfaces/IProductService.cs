using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Models;

namespace UXComex.GerenciadorPedidos.Domain.Interfaces
{
    public interface IProductService
    {
        Task<int> CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<PagedResult<Product>> SearchAsync(string searchTerm, int pageNumber, int pageSize);
        Task UpdateStockAsync(int productId, int quantity);
    }
}
