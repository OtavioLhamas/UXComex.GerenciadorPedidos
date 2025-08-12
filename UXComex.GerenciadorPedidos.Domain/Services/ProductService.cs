using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> CreateAsync(Product product)
        {
            // Perform business logic validation
            Validate(product);

            return await _productRepository.AddAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<PagedResult<Product>> SearchAsync(string searchTerm, int pageNumber, int pageSize)
        {
            return await _productRepository.SearchAndPaginateAsync(searchTerm, pageNumber, pageSize);
        }

        public async Task UpdateAsync(Product product)
        {
            // Perform business logic validation
            Validate(product);

            var existingProduct = _productRepository.GetByIdAsync(product.Id)
                ?? throw new Exception("Product not found.");

            await _productRepository.UpdateAsync(product);
        }

        public async Task UpdateStockAsync(int productId, int quantity)
        {
            await _productRepository.UpdateStockAsync(productId, quantity);
        }

        /// <summary>
        /// Validates the Product object based on business rules.
        /// </summary>
        /// <param name="product"></param>
        private static void Validate(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name is required.", nameof(product.Name));
            }

            if (product.Price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero.", nameof(product.Price));
            }

            if (product.StockQuantity < 0)
            {
                throw new ArgumentException("Product stock quantity cannot be negative.", nameof(product.StockQuantity));
            }

            // TODO:
            // - Check for a minimum/maximum length for the name and description.
        }
    }
}
