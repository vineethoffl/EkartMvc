using EkartMvc.Repositories.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EkartMvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public IActionResult Product()
        {
            var products = _productRepository.GetProducts();
            return View(products);
        }
    }
}
