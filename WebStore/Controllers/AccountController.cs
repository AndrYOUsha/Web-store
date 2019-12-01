using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Models.Identity;
using WebStore.Models.Identity.IdentityViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        ProductContext _context;

        public AccountController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            TempData["PrevPage"] = Request.Headers["Referer"].ToString();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        MiddleName = model.MiddleName,
                        LastName = model.LastName,
                        Age = model.Age,
                        Email = model.Email,
                        Login = model.Login,
                        Password = model.Password,
                        Phone = model.Phone,
                        DateRegistered = DateTime.Now
                    };
                    var userRole = await _context.Roles.FirstOrDefaultAsync(u => u.Name == "user");
                    user.Role = userRole;

                    try
                    {
                        await _context.AddAsync(user);
                        await _context.SaveChangesAsync();

                        await Authenticate(user);

                        return Redirect(TempData["PrevPage"].ToString());
                    }
                    catch
                    {
                        return Content("Ошибка при сохранении.");
                    }
                }
                else
                    ModelState.AddModelError("", "Такой email уже существует");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            TempData["PrevPage"] = Request.Headers["Referer"].ToString();
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Redirect(TempData["PrevPage"].ToString());
            }

            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    await Authenticate(user);

                    return Redirect(TempData["PrevPage"].ToString());
                }
                else
                    ModelState.AddModelError("", "Неправильные данные ввода");

            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };

            var id = new ClaimsIdentity(claims, 
                "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

    }
}