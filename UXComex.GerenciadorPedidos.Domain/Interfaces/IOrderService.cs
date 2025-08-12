using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Models;
using UXComex.GerenciadorPedidos.Domain.Services;

namespace UXComex.GerenciadorPedidos.Domain.Interfaces
{
    public interface IOrderService
    {
        Task CreateAsync(Order order, List<OrderItem> items);
        Task<Order> GetByIdAsync(int id);
        Task<PagedResult<Order>> SearchAsync(int? clientId, OrderStatus? status, int pageNumber, int pageSize);
        Task UpdateStatusAsync(int orderId, OrderStatus newStatus);

    }
}