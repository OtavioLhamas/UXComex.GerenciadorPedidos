using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Models;

namespace UXComex.GerenciadorPedidos.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> CreateWithItemsAsync(Order order);
        Task UpdateStatusAsync(Order order);
        Task<Order> GetByIdAsync(int id);
        Task<PagedResult<Order>> SearchAndPaginateAsync(int? clientId, OrderStatus? status, int pageNumber, int pageSize);
    }
}
