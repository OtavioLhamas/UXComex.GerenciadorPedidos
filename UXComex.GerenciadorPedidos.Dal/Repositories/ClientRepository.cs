using Dapper;
using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Interfaces;
using UXComex.GerenciadorPedidos.Domain.Models;

namespace UXComex.GerenciadorPedidos.Dal.Repositories
{
    /// <summary>
    /// Concrete implementation of IClientRepository using Dapper.
    /// This class handles all the raw SQL queries and data mapping for the Client entity.
    /// </summary>
    public class ClientRepository(IDbConnectionFactory connectionFactory) : IClientRepository
    {
        private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

        /// <summary>
        /// Get all clients from the database, ordered by Name.
        /// </summary>
        /// <returns>
        /// A list of all Clients.
        /// </returns>
        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            var sql = "SELECT * FROM Clients ORDER BY Name;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Client>(sql);
            }
        }

        /// <summary>
        /// Search clients by name or email.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns>
        /// A list of clients that match the search.
        /// </returns>
        public async Task<IEnumerable<Client>> SearchAsync(string searchTerm)
        {
            var sql = "SELECT * FROM Clients WHERE Name LIKE @SearchTerm OR Email LIKE @SearchTerm ORDER BY Name;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Client>(sql, new { SearchTerm = $"%{searchTerm}%" });
            }
        }

        /// <summary>
        /// Search clients based on name or email and paginates the results.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>Filtered list of clients with the pagination info.</returns>
        public async Task<PagedResult<Client>> SearchAndPaginateAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var sql = "SELECT COUNT(*) FROM Clients WHERE Name LIKE @SearchTerm OR Email LIKE @SearchTerm;" +
                      "SELECT * FROM Clients WHERE Name LIKE @SearchTerm OR Email LIKE @SearchTerm " +
                      "ORDER BY Name OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var offset = (pageNumber - 1) * pageSize;
            var parameters = new { SearchTerm = $"%{searchTerm}%", Offset = offset, PageSize = pageSize };

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var multi = await connection.QueryMultipleAsync(sql, parameters))
                {
                    var totalCount = await multi.ReadSingleAsync<int>();
                    var clients = await multi.ReadAsync<Client>();

                    return new PagedResult<Client>
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
        /// Get a single client by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The client object or null.</returns>
        public async Task<Client> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Clients WHERE Id = @Id;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Client>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Get a single client by Email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>The client object or null.</returns>
        public async Task<Client> GetByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Clients WHERE Email = @Email;";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Client>(sql, new { Email = email });
            }
        }

        /// <summary>
        /// Create a new client.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>The new client Id</returns>
        public async Task<int> AddAsync(Client client)
        {
            var sql = @"INSERT INTO Clients (Name, Email, Phone) 
                        OUTPUT INSERTED.Id 
                        VALUES (@Name, @Email, @Phone);";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(sql, client);
            }
        }

        /// <summary>
        /// Update the info of an existing client by Id.
        /// </summary>
        /// <param name="client">The client object containing the updated info.</param>
        /// <returns></returns>
        public async Task UpdateAsync(Client client)
        {
            var sql = "UPDATE Clients SET Name = @Name, Email = @Email, Phone = @Phone WHERE Id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(sql, client);
            }
        }

        /// <summary>
        /// Delete a client by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM Clients WHERE Id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
