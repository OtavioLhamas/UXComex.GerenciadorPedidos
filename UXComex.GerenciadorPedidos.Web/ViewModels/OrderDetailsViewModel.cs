using UXComex.GerenciadorPedidos.Domain.Entities;

namespace UXComex.GerenciadorPedidos.Web.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        public ClientViewModel Client { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal TotalValue { get; set; }
        public string Status { get; set; }
        public List<OrderItemDetailsViewModel> Items { get; set; }
    }

    public class OrderItemDetailsViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}