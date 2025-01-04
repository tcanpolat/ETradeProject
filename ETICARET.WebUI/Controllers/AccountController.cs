using ETICARET.Business.Abstract;
using ETICARET.WebUI.EmailService;
using ETICARET.WebUI.Extensions;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private ICartService _cartService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ICartService cartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartService = cartService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded) 
            {
                // generate email token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });

                string siteUrl = "https://localhost:5174";
                string activeUrl = $"{siteUrl}{callbackUrl}";

                string body = $"Hesabınızı onaylayınız. <br> <br> Lütfen email hesabını onaylamak için linke <a href='{activeUrl}'> tıklayınız.</a>";

                // Email Service 
                MailHelper.SendEmail(body,model.Email,"ETİCARET Hesap Aktifleştirme Onayı");

                return RedirectToAction("Login","Account");
            
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if(userId == null || token == null)
            {
                TempData.Put("message", new ResultModel()
                {
                    Title = "Geçersiz Token",
                    Message = "Hesap onay bilgileri yanlış",
                    Css = "danger"
                });

                return Redirect("~");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token); // email confirmed ı 1 yapar.

                if (result.Succeeded)
                {
                    _cartService.InitialCart(userId);

                    TempData.Put("message", new ResultModel() 
                    { 
                        Title = "Hesap Onayı",
                        Message = "Hesabınız onaylanmıştır",
                        Css = "success"
                    });

                    return RedirectToAction("Login", "Account");

                }

            }

            TempData.Put("message", new ResultModel()
            {
                Title = "Hesap Onayı",
                Message = "Hesabınız onaylanmamıştır",
                Css = "danger"
            });

            return Redirect("~");
        }
    }
}
