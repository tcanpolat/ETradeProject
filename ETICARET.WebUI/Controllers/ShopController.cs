using ETICARET.Business.Abstract;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("products/{category?}")]
        public IActionResult List(string category,int page = 1)
        {
            const int pageSize = 5;

            var products = new ProductListModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    ItemsPerPage = pageSize,
                    CurrentCategory = category,
                    CurrentPage = page
                },
                Products = _productService.GetProductByCategory(category,page,pageSize)
            };

            return View(products);
        }
    }
}
