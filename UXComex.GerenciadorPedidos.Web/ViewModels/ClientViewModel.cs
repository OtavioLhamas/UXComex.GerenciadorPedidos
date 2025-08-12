using System.ComponentModel.DataAnnotations;
using UXComex.GerenciadorPedidos.Domain.Entities;

namespace UXComex.GerenciadorPedidos.Web.ViewModels
{
    public class ClientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Client name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }

        public string? Phone { get; set; }

        public DateTime RegistrationDate { get; set; }

        public static implicit operator ClientViewModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                RegistrationDate = client.RegistrationDate
            };
        }

        public static implicit operator Client(ClientViewModel viewModel)
        {
            return new Client
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                RegistrationDate = viewModel.RegistrationDate
            };
        }
    }
}