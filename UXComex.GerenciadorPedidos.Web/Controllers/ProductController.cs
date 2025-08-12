using Microsoft.AspNetCore.Mvc;
using UXComex.GerenciadorPedidos.Domain.Entities;
using UXComex.GerenciadorPedidos.Domain.Interfaces;
using UXComex.GerenciadorPedidos.Web.ViewModels;

namespace UXComex.GerenciadorPedidos.Web.Controllers
{
    /// <summary>
    /// Controller for managing product CRUD operations using the ClientService.
    /// </summary>
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService clientService)
        {
            _productService = clientService;
        }

        // GET: /Product
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Product/GetProducts
        [HttpGet]
        public async Task<JsonResult> GetProducts(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var pagedResult = await _productService.SearchAsync(searchTerm, pageNumber, pageSize);
            return Json(pagedResult);
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.CreateAsync((Product)productViewModel);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(productViewModel);
        }

        // GET: /Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = (ProductViewModel)product;
            return View(productViewModel);
        }

        // POST: /Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = (Product)productViewModel;
                    await _productService.UpdateAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(productViewModel);
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }
    }
}