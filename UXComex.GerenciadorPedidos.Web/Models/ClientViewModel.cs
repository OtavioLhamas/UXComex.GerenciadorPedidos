using System.ComponentModel.DataAnnotations;
using UXComex.GerenciadorPedidos.Domain.Entities;

namespace UXComex.GerenciadorPedidos.Web.ViewModels
{
    /// <summary>
    /// ViewModel for the Client entity.
    /// It's used to transfer data between the controller and the views,
    /// providing a clean separation of concerns and preventing over-posting.
    /// It also contains presentation-specific validation attributes.
    /// </summary>
    public class ClientViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Client name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public string Phone { get; set; }

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