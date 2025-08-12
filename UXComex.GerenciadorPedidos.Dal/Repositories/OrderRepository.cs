using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Interfaces;
using UXComex.GerenciadorPedidos.Domain.Models;

namespace UXComex.GerenciadorPedidos.Dal.Repositories
{
    /// <summary>
    /// Concrete implementation of IOrderRepository using Dapper.
    /// This class handles all the raw SQL queries and data mapping for the Order entity.
    /// </summary>
    public class OrderRepository(IDbConnectionFactory connectionFactory) : IOrderRepository
    {
        private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

        /// <summary>
        /// Create a new order with its items, update product stock, and log the creation.
        /// </summary>
        /// <param name="order"></param>
        /// <returns>The new order Id</returns>
        public async Task<int> CreateWithItemsAsync(Order order)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var orderSql = "INSERT INTO Orders (ClientId, CreationDate, TotalValue, Status) VALUES (@ClientId, @CreationDate, @TotalValue, @Status); SELECT CAST(SCOPE_IDENTITY() as int)";
                        int newOrderId = await connection.QuerySingleAsync<int>(orderSql, new { order.ClientId, order.CreationDate, order.TotalValue, Status = order.Status.ToString() }, transaction);

                        var orderItemSql = "INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice)";
                        foreach (var item in order.Items)
                        {
                            item.OrderId = newOrderId;
                            await connection.ExecuteAsync(orderItemSql, item, transaction);
                        }

                        // Update product stock
                        var updateStockSql = "UPDATE Products SET StockQuantity = StockQuantity - @Quantity WHERE Id = @ProductId";
                        foreach (var item in order.Items)
                        {
                            await connection.ExecuteAsync(updateStockSql, new { item.Quantity, item.ProductId }, transaction);
                        }

                        // Simulate notification of a new order
                        // TODO: Create Notifications table and use logger.
                        var logSql = "INSERT INTO Notifications (Message, CreatedDate) VALUES (@Message, GETDATE())";
                        var logMessage = $"New order created with ID {newOrderId} for client ID {order.ClientId}.";
                        await connection.ExecuteAsync(logSql, new { Message = logMessage }, transaction);

                        transaction.Commit();

                        return newOrderId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Get the details of a single order by Id, including client and items.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Order> GetByIdAsync(int id)
        {
            var sql = @"
                    SELECT O.*, C.Id as ClientId, C.Name, C.Email, C.Phone, C.RegistrationDate, OI.Id as OrderItemId, OI.ProductId, OI.Quantity, OI.UnitPrice
                    FROM Orders O
                    JOIN Clients C ON O.ClientId = C.Id
                    LEFT JOIN OrderItems OI ON O.Id = OI.OrderId
                    WHERE O.Id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var items = await connection.QueryAsync<Order, Client, OrderItem, Order>(
                    sql,
                    (order, client, orderItem) =>
                    {
                        order.Client = client;
                        order.Items.Add(orderItem);
                        return order;
                    },
                    new { Id = id },
                    splitOn: "ClientId, OrderItemId");

                var result = items.GroupBy(o => o.Id).Select(g =>
                {
                    var groupedOrder = g.First();
                    groupedOrder.Items = g.Select(g => g.Items.Single()).ToList();
                    return groupedOrder;
                });

                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// Search orders based on client Id and status and paginates the results.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="status"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Filtered list of orders with the pagination info.</returns>
        public async Task<PagedResult<Order>> SearchAndPaginateAsync(int? clientId, OrderStatus? status, int pageNumber, int pageSize)
        {
            var sql = @"
                SELECT COUNT(O.Id)
                FROM Orders O
                JOIN Clients C ON O.ClientId = C.Id
                WHERE (@ClientId IS NULL OR O.ClientId = @ClientId)
                AND (@Status IS NULL OR O.Status = @Status);

                SELECT O.*, C.Name as ClientName
                FROM Orders O
                JOIN Clients C ON O.ClientId = C.Id
                WHERE (@ClientId IS NULL OR O.ClientId = @ClientId)
                AND (@Status IS NULL OR O.Status = @Status)
                ORDER BY O.CreationDate DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            var offset = (pageNumber - 1) * pageSize;
            var parameters = new { ClientId = clientId, Status = status?.ToString(), Offset = offset, PageSize = pageSize };

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var multi = await connection.QueryMultipleAsync(sql, parameters))
                {
                    var totalCount = await multi.ReadSingleAsync<int>();
                    var orders = (await multi.ReadAsync<Order>()).ToList();

                    foreach (var order in orders)
                    {
                        var client = await connection.QuerySingleAsync<Client>(
                            "SELECT * FROM Clients WHERE Id = @ClientId",
                            new { order.ClientId });
                        order.Client = client;
                    }

                    return new PagedResult<Order>
                    {
                        Items = orders,
                        TotalCount = totalCount,
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    };
                }
            }
        }

        /// <summary>
        /// Update the info of an existing product by Id.
        /// </summary>
        /// <param name="order">The product object containing the updated info.</param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(Order order)
        {
            var sql = "UPDATE Orders SET Status = @Status WHERE Id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { order.Id, Status = order.Status.ToString() });
            }
        }
    }
}
