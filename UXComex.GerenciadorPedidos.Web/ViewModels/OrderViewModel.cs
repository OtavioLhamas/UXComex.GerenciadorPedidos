using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace UXComex.GerenciadorPedidos.Web.ViewModels
{
    /// <summary>
    /// ViewModel for the Order creation process.
    /// </summary>
    public class OrderViewModel
    {
        [Display(Name = "Client")]
        [Required(ErrorMessage = "A client must be selected.")]
        public int ClientId { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();

        [ValidateNever]
        public IEnumerable<ClientViewModel> AvailableClients { get; set; }
        [ValidateNever]
        public IEnumerable<ProductViewModel> AvailableProducts { get; set; }
    }
}
