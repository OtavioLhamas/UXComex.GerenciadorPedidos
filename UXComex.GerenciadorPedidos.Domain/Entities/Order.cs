using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UXComex.GerenciadorPedidos.Domain.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public DateTime CreationDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
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