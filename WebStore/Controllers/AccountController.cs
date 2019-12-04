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
using WebStore.AdditionalLogic;

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
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    var userRole = await _context.Roles
                        .FirstOrDefaultAsync(u => u.Name == "user");
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
                        DateRegistered = DateTime.Now,
                        Role = userRole
                    };

                    try
                    {
                        await _context.AddAsync(user);
                        await _context.SaveChangesAsync();

                        string tempkey = TempKey();
                        Response.Cookies.Append("tempData", tempkey);

                        if (Request.Cookies.ContainsKey("tempData"))
                        {
                            var callbackUrl = Url.Action(
                                "Confirmation",
                                "Account",
                                new { userId = user.ID, code = tempkey },
                                protocol: HttpContext.Request.Scheme);
                            EmailService emailService = new EmailService();
                            await emailService.SendEmailAsync(user.Email, "Подтвердите ваш аккаунт",
                                $"Подтвердите регистрацию на сайте \"WebStore\", перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                            return RedirectToAction("Confirmation", "Account");
                        }
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
                    if (user.ConfirmEmail)
                        await Authenticate(user);
                    else
                        return RedirectToAction("Confirmation", "Account", new { userId = user.ID });

                    if ((TempData["PrevPage"].ToString() != null) && (TempData["PrevPage"].ToString() != ""))
                        return Redirect(TempData["PrevPage"].ToString());

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Email и/или пароль не верны");

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(int? userId, string code)
        {
            if ((userId != null) && (code != null))
            {
                if (code != Request.Cookies["tempData"])
                {
                    ViewData["Title"] = "Нам не удалось подтвердить вашу электронную почту, попробуйте снова";
                    return View();
                }

                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.ID == userId);

                if ((user != null))
                {
                    Response.Cookies.Delete("tempData");
                    ViewData["Title"] = "Поздравляем! Вы успешно подтвердили электронную почту!";

                    user.ConfirmEmail = true;
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    await Authenticate(user);
                    return View();
                }
            }

            ViewData["Title"] = "Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Confirmation(int? userId)
        {
            if (userId != null)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.ID == userId);

                string tempkey = TempKey();
                Response.Cookies.Append("tempData", tempkey);

                if (Request.Cookies.ContainsKey("tempData"))
                {
                    var callbackUrl = Url.Action(
                        "Confirmation",
                        "Account",
                        new { userId = user.ID, code = tempkey },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(user.Email, "Подтвердите ваш аккаунт",
                        $"Подтвердите регистрацию на сайте \"WebStore\", перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                    ViewData["Title"] = "Поздравляем! Вы успешно подтвердили электронную почту!";
                    return RedirectToAction("Confirmation", "Account");
                }
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult RecoveryPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoveryPassword(RecoveryModel model)
        {
            if (model.Email != null)
            {
                var user = await _context.Users
                    .FirstAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Такого Email нет в базе данный");
                    return View(model);
                }

                var callbackUrl = Url.Action(
                    "Login",
                    "Account",
                    null,
                    protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(user.Email, "Ваш пароль на сайте \"WebStore\"",
                    $"Ваш пароль на сайте \"WebStore\": <span style=\"background-color: #faebd7\"> {user.Password} </span> <br /> <a href='{callbackUrl}'>Вернуться на сайт</a>");
            }
            return RedirectToAction("Login", "Account");
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

        private string TempKey()
        {
            string result;

            var random = new Random();
            result = random.Next(100000, 999999).ToString();

            return result;
        }
    }
}