using Dapper;
using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Interfaces;
using UXComex.GerenciadorPedidos.Domain.Models;

namespace UXComex.GerenciadorPedidos.Dal.Repositories
{
    /// <summary>
    /// Concrete implementation of IProductRepository using Dapper.
    /// This class handles all the raw SQL queries and data mapping for the Product entity.
    /// </summary>
    public class ProductRepository(IDbConnectionFactory connectionFactory) : IProductRepository
    {
        private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

        /// <summary>
        /// Get all products from the database, ordered by Name.
        /// </summary>
        /// <returns>
        /// A list of all Products.
        /// </returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var sql = "SELECT * FROM Products ORDER BY Name;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Product>(sql);
            }
        }

        /// <summary>
        /// Search products by name.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns>
        /// A list of products that match the search.
        /// </returns>
        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            var sql = "SELECT * FROM Products WHERE Name LIKE @SearchTerm ORDER BY Name;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Product>(sql, new { SearchTerm = $"%{searchTerm}%" });
            }
        }

        /// <summary>
        /// Searches and paginates products.
        /// </summary>
        public async Task<PagedResult<Product>> SearchAndPaginateAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var sql = "SELECT COUNT(*) FROM Products WHERE Name LIKE @SearchTerm;" +
                      "SELECT * FROM Products WHERE Name LIKE @SearchTerm " +
                      "ORDER BY Name OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var offset = (pageNumber - 1) * pageSize;
            var parameters = new { SearchTerm = $"%{searchTerm}%", Offset = offset, PageSize = pageSize };

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var multi = await connection.QueryMultipleAsync(sql, parameters))
                {
                    var totalCount = await multi.ReadSingleAsync<int>();
                    var clients = await multi.ReadAsync<Product>();

                    return new PagedResult<Product>
                    {
                        Items = clients,
                        TotalCount = totalCount,
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    };
                }
            }
        }
        /// <summary>
        /// Get a single product by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The product object or null.</returns>
        public async Task<Product> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Products WHERE Id = @Id;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>The new product Id</returns>
        public async Task<int> AddAsync(Product product)
        {
            var sql = @"INSERT INTO Products (Name, Description, Price, StockQuantity) 
                        OUTPUT INSERTED.Id 
                        VALUES (@Name, @Description, @Price, @StockQuantity);";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(sql, product);
            }
        }

        /// <summary>
        /// Update the info of an existing product by Id.
        /// </summary>
        /// <param name="product">The product object containing the updated info.</param>
        /// <returns></returns>
        public async Task UpdateAsync(Product product)
        {
            var sql = "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, StockQuantity = @StockQuantity WHERE Id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(sql, product);
            }
        }

        /// <summary>
        /// Delete a product by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM Products WHERE Id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Subtracts the specified quantity from the stock of a product.
        /// </summary>
        /// <param name="productId">The Id of the product to be updated</param>
        /// <param name="quantity">The quantity to be removed from the stock</param>
        /// <returns></returns>
        public async Task UpdateStockAsync(int productId, int quantity)
        {
            var sql = "UPDATE Products SET StockQuantity = StockQuantity - @Quantity WHERE Id = @ProductId;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { ProductId = productId, Quantity = quantity });
            }
        }
    }
}
