using Microsoft.AspNetCore.Mvc;
using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Interfaces;
using UXComex.GerenciadorPedidos.Web.ViewModels;

namespace UXComex.GerenciadorPedidos.Web.Controllers
{
    /// <summary>
    /// Controller for managing order CRUD operations using the OrderService.
    /// This includes creating new orders and viewing existing ones.
    /// </summary>
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IClientService _clientService;
        private readonly IProductService _productService;

        public OrderController(IOrderService orderService, IClientService clientService, IProductService productService)
        {
            _orderService = orderService;
            _clientService = clientService;
            _productService = productService;
        }

        // GET: /Order
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllAsync();
            var viewModel = new OrderFilterViewModel
            {
                AvailableClients = clients.Select(c => (ClientViewModel)c).ToList(),
                AvailableStatuses = Enum.GetNames(typeof(OrderStatus)).ToList()
            };
            return View(viewModel);
        }

        // GET: /Order/GetOrders
        [HttpGet]
        public async Task<JsonResult> GetOrders(int? clientId, OrderStatus? status, int pageNumber = 1, int pageSize = 10)
        {
            var pagedResult = await _orderService.SearchAsync(clientId, status, pageNumber, pageSize);

            return Json(pagedResult);
        }

        // GET: /Order/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new OrderDetailsViewModel
            {
                Id = order.Id,
                Client = (ClientViewModel)order.Client,
                CreationDate = order.CreationDate,
                TotalValue = order.TotalValue,
                Status = order.Status.ToString(),
            };

            viewModel.Items = new List<OrderItemDetailsViewModel>();

            foreach (var item in order.Items)
            {
                var product = await _productService.GetByIdAsync(item.ProductId);
                viewModel.Items.Add(new OrderItemDetailsViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = product?.Name,
                    ProductDescription = product?.Description,
                    Quantity = item.Quantity,
                    Price = item.UnitPrice,
                    TotalPrice = item.UnitPrice * item.Quantity
                });
            }

            return View(viewModel);
        }

        // POST: /Order/ChangeStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int orderId, OrderStatus newStatus)
        {
            try
            {
                await _orderService.UpdateStatusAsync(orderId, newStatus);
                return Json(new { success = true, message = "Order status updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: /Order/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new OrderViewModel
            {
                AvailableClients = (await _clientService.GetAllAsync()).Select(c => (ClientViewModel)c),
                AvailableProducts = (await _productService.GetAllAsync()).Select(p => (ProductViewModel)p)
            };

            return View(viewModel);
        }

        // POST: /Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId", "OrderItems")] OrderViewModel viewModel)
        {
            // Manually validate order items, as they are not bound directly by the form.
            if (viewModel.OrderItems == null || viewModel.OrderItems.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "An order must have at least one item.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var order = new Order { ClientId = viewModel.ClientId };
                    var orderItems = viewModel.OrderItems.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });

                    await _orderService.CreateAsync(order, orderItems.ToList());
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // If the model state is invalid, re-populate the dropdowns and return the view
            viewModel.AvailableClients = (await _clientService.GetAllAsync()).Select(c => (ClientViewModel)c);
            viewModel.AvailableProducts = (await _productService.GetAllAsync()).Select(p => (ProductViewModel)p);

            return View(viewModel);
        }

        // GET: /Order/GetProductDetails
        [HttpGet]
        public async Task<JsonResult> GetProductDetails(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return Json(null);
            }
            return Json(new { price = product.Price, stockQuantity = product.StockQuantity });
        }
    }
}