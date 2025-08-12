using Microsoft.AspNetCore.Mvc;
using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Services;
using UXComex.GerenciadorPedidos.Web.ViewModels;

namespace UXComex.GerenciadorPedidos.Web.Controllers
{
    /// <summary>
    /// Controller for managing client CRUD operations using the ClientService.
    /// </summary>
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // GET: /Client
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Client/GetClients
        [HttpGet]
        public async Task<JsonResult> GetClients(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var pagedResult = await _clientService.SearchClientsAsync(searchTerm, pageNumber, pageSize);
            return Json(pagedResult);
        }

        // GET: /Client/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Client/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientViewModel clientViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _clientService.CreateClientAsync((Client)clientViewModel);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(clientViewModel);
        }

        // GET: /Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientService.GetClientByIdAsync(id.Value);
            if (client == null)
            {
                return NotFound();
            }

            var clientViewModel = (ClientViewModel)client;
            return View(clientViewModel);
        }

        // POST: /Client/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientViewModel clientViewModel)
        {
            if (id != clientViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var client = (Client)clientViewModel;
                    await _clientService.UpdateClientAsync(client);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(clientViewModel);
        }

        // POST: /Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return Ok();
        }
    }
}