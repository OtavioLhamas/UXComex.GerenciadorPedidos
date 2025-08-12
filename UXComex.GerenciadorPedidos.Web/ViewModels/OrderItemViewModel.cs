using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UXComex.GerenciadorPedidos.Domain.Entities;

namespace UXComex.GerenciadorPedidos.Web.ViewModels
{
    /// <summary>
    /// ViewModel for an individual item within an order.
    /// </summary>
    [Bind(include: "ProductId,Quantity")]
    public class OrderItemViewModel
    {
        [Required(ErrorMessage = "A product must be selected.")]
        public int ProductId { get; set; }
        [ValidateNever]
        public string? ProductName { get; set; }
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
    }

}
