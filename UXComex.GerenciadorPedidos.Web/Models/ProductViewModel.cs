
using System.ComponentModel.DataAnnotations;
using UXComex.GerenciadorPedidos.Domain.Entities;

namespace UXComex.GerenciadorPedidos.Web.ViewModels
{
    /// <summary>
    /// ViewModel for the Product entity.
    /// Used to transfer data between the controller and the views.
    /// It includes validation attributes for presentation layer validation.
    /// </summary>
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(255, ErrorMessage = "Product name cannot exceed 255 characters.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, 999999, ErrorMessage = "Stock must be a non-negative number.")]
        public int StockQuantity { get; set; }

        public static implicit operator ProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };
        }

        public static implicit operator Product(ProductViewModel viewModel)
        {
            return new Product
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                StockQuantity = viewModel.StockQuantity
            };
        }
    }
}
