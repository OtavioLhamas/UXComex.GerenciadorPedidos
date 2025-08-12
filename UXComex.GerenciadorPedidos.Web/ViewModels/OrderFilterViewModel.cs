namespace UXComex.GerenciadorPedidos.Web.ViewModels
{
    public class OrderFilterViewModel
    {
        public List<ClientViewModel> AvailableClients { get; set; }
        public List<string> AvailableStatuses { get; set; }
    }
}
