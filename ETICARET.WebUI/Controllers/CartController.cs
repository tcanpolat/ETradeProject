using ETICARET.Business.Abstract;
using ETICARET.WebUI.Extensions;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IProductService _productService;
        private IOrderService _orderService;
        private UserManager<ApplicationUser> _userManager;

        public CartController(ICartService cartService,IProductService productService,IOrderService orderService,UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _productService = productService;
            _orderService = orderService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            return View(
                new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(x => new CartItemModel()
                    {
                        CartItemId = x.Id,
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        Price = x.Product.Price,
                        ImageUrl = x.Product.Images[0].ImageUrl,
                        Quantity = x.Quantity
                    }).ToList()
                }
                );
        }

        public IActionResult AddToCart(int productId,int quantity)
        {
            _cartService.AddToCart(_userManager.GetUserId(User), productId, quantity); 
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFromCart(int productId)
        {
            _cartService.DeleteFromCart(_userManager.GetUserId(User), productId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            OrderModel orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(x => new CartItemModel()
                {
                    CartItemId = x.Id,
                    ProductId = x.Product.Id,
                    Name = x.Product.Name,
                    Price = x.Product.Price,
                    ImageUrl = x.Product.Images[0].ImageUrl,
                    Quantity = x.Quantity
                }).ToList()
            };

            return View(orderModel);
        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model, string paymentMethod)
        {
            ModelState.Remove("CartModel");

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);

                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(x => new CartItemModel()
                    {
                        CartItemId = x.Id,
                        ProductId = x.Product.Id,
                        Name = x.Product.Name,
                        Price = x.Product.Price,
                        ImageUrl = x.Product.Images[0].ImageUrl,
                        Quantity = x.Quantity
                    }).ToList()
                };

                if(paymentMethod == "credit")
                {
                    var payment = PaymentProccess(model);

                    if(payment.Status == "success")
                    {
                        // Siparişi kaydet
                        // Sepeti temizle
                        TempData.Put("message", new ResultModel()
                        {
                            Title = "Order Completed",
                            Message = "Your order has been completed successfully",
                            Css = "success"
                        });
                    }
                    else
                    {
                        TempData.Put("message", new ResultModel()
                        {
                            Title = "Order Uncompleted",
                            Message = "Your order could not be completed",
                            Css = "danger"
                        });
                    }
                }
                else
                {
                    var payment = PaymentProccess(model);

                    if (payment.Status == "success")
                    {
                        // Siparişi kaydet
                        // Sepeti temizle
                        TempData.Put("message", new ResultModel()
                        {
                            Title = "Order Completed",
                            Message = "Your order has been completed successfully",
                            Css = "success"
                        });
                    }
                    else
                    {
                        TempData.Put("message", new ResultModel()
                        {
                            Title = "Order Uncompleted",
                            Message = "Your order could not be completed",
                            Css = "danger"
                        });
                    }
                }
               
            }
        }

        private bool PaymentProccess(OrderModel model)
        {
            throw new NotImplementedException();
        }
    }
}
