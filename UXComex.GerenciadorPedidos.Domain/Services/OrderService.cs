using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Interfaces;
using UXComex.GerenciadorPedidos.Domain.Models;

namespace UXComex.GerenciadorPedidos.Domain.Services
{
    /// <summary>
    /// Implements the IOrderService interface with business logic.
    /// This is where the core business logic, including validation and
    /// error handling, is executed before interacting with the repository.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IClientRepository clientRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _clientRepository = clientRepository;
        }

        public async Task CreateAsync(Order order, List<OrderItem> items)
        {
            await Validate(order, items);

            // Calculate the total value of the order
            order.TotalValue = items.Sum(item => item.UnitPrice * item.Quantity);

            // The database has the default value for CreationDate and Status,
            // but for demonstration purposes, we set it here.
            order.CreationDate = DateTime.Now;
            order.Status = OrderStatus.New;

            order.Items = items;

            await _orderRepository.CreateWithItemsAsync(order);
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<PagedResult<Order>> SearchAsync(int? clientId, OrderStatus? status, int pageNumber, int pageSize)
        {
            return await _orderRepository.SearchAndPaginateAsync(clientId, status, pageNumber, pageSize);
        }

        private async Task Validate(Order order, List<OrderItem> items)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            var client = await _clientRepository.GetByIdAsync(order.ClientId);

            if (client == null)
            {
                throw new Exception("Client not found. A valid client must be selected for the order.");
            }

            if (items == null || !items.Any())
            {
                throw new Exception("An order must have at least one item.");
            }

            // Perform stock validation before attempting to create the order
            foreach (OrderItem item in items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new ArgumentException($"Product with ID {item.ProductId} not found.", nameof(item.ProductId));

                if (product.StockQuantity < item.Quantity)
                {
                    throw new Exception($"Insufficient stock for product '{product.Name}'. Available: {product.StockQuantity}, Requested: {item.Quantity}.");
                }

                // Add the actual price from the database to prevent tampering
                item.UnitPrice = product.Price;
            }
        }

        public async Task UpdateStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId)
                ?? throw new Exception($"Order with ID {orderId} not found.");

            if (!Enum.IsDefined(typeof(OrderStatus), newStatus))
            {
                throw new ArgumentException("Invalid order status.", nameof(newStatus));
            }

            if (order.Status == newStatus)
            {
                throw new Exception($"The order is already in the '{newStatus}' status.");
            }

            order.Status = newStatus;
            await _orderRepository.UpdateStatusAsync(order);
        }
    }
}
