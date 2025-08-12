namespace UXComex.GerenciadorPedidos.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalValue { get; set; }
        public OrderStatus Status { get; set; }

        // Navigation property for order items
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    public enum OrderStatus
    {
        New,
        Processing,
        Finished,
        Cancelled
    }
}