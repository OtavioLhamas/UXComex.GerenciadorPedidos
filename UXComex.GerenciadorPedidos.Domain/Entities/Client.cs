using System.ComponentModel.DataAnnotations;

namespace UXComex.GerenciadorPedidos.Domain.Entities
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
