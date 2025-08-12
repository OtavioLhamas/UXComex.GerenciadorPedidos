using System.ComponentModel.DataAnnotations;
using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Interfaces;
using UXComex.GerenciadorPedidos.Domain.Models;

namespace UXComex.GerenciadorPedidos.Domain.Services
{
    /// <summary>
    /// Implements the IClientService interface.
    /// This is where the core business logic, including validation and
    /// error handling, is executed before interacting with the repository.
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<int> CreateAsync(Client client)
        {
            // Perform business logic validation
            Validate(client);

            // Additional business logic (e.g., check for email uniqueness)
            
            // We can instead leave this validation to the database with a unique constraint,
            // but for a custom message demonstration, we'll do a simple check here.
            var existingClient = await _clientRepository.GetByEmailAsync(client.Email);
            if (existingClient != null)
            {
                throw new Exception("A client with this email already exists.");
            }
            // The database has the default value for RegistrationDate,
            // but again, for demonstration purposes, we set it here.
            client.RegistrationDate = DateTime.Now;

            return await _clientRepository.AddAsync(client);
        }

        public async Task UpdateAsync(Client client)
        {
            // Perform business logic validation
            Validate(client);

            // Check if the client exists before updating
            var existingClient = await _clientRepository.GetByIdAsync(client.Id) ?? throw new Exception("Client not found.");
            
            await _clientRepository.UpdateAsync(client);
        }

        public async Task DeleteAsync(int id)
        {
            await _clientRepository.DeleteAsync(id);
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _clientRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _clientRepository.GetAllAsync();
        }

        /// <summary>
        /// Retrieves a paginated list of clients with an optional search term.
        /// </summary>
        public async Task<PagedResult<Client>> SearchAsync(string searchTerm, int pageNumber, int pageSize)
        {
            return await _clientRepository.SearchAndPaginateAsync(searchTerm, pageNumber, pageSize);
        }

        /// <summary>
        /// Validates the Client object based on business rules.
        /// </summary>
        public static void Validate(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.Name))
            {
                throw new ArgumentException("Client name is required.", nameof(client.Name));
            }

            if (string.IsNullOrWhiteSpace(client.Email))
            {
                throw new ArgumentException("Client email is required.", nameof(client.Email));
            }

            if (!new EmailAddressAttribute().IsValid(client.Email))
            {
                throw new ArgumentException("Invalid email format.", nameof(client.Email));
            }

            // TODO:
            // - Check for a minimum/maximum length for the name.
            // - Validate the phone number format.
        }
    }
}