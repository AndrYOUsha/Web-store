using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;
using WebStore.Models.ProductViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using WebStore.Patterns;
using WebStore.Patterns.StrategyPattern;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        ProductContext _context;
        Strategy strategy;

        public AdminController(ProductContext context)
        {
            _context = context;
            strategy = new Strategy();
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ProductCharacteristicViewModel();

            viewModel.Products = await _context.Products
                .Include(p => p.Characteristic)
                .AsNoTracking()
                .ToListAsync();

            if (User.Identity.IsAuthenticated)
                ViewData["Identity"] = User.FindFirst(u => u.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            return View(viewModel);
        }

        //Создать продукт и характристику
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
                return View();

            product.DateOfAppearances = DateTime.Now;
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Characteristic)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Characteristics(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = new ProductCharacteristicViewModel();

            viewModel.Characteristics = await _context.Characteristics
                .Where(item => item.Product.ID == id)
                .Include(item => item.Product)
                .AsNoTracking()
                .ToListAsync();

            viewModel.Count = Count(viewModel.Characteristics);

            if (!viewModel.Characteristics.Any())
                return RedirectToAction(nameof(Index));

            var product = viewModel.Characteristics.First().Product;

            ViewData["TitleCharacteristics"] = $"Таблица характеристик товара \"{product.Title}\"";

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Characteristics(Characteristic characteristic)
        {
            var charact = await _context.Characteristics
                .FirstOrDefaultAsync(item => item.ID == characteristic.ID);

            if (charact == null)
                return NotFound();

            charact.Article = characteristic.Article;
            charact.Brand = characteristic.Brand;
            charact.Brunch = characteristic.Brunch;
            charact.Color = characteristic.Color;
            charact.FullName = characteristic.FullName;
            charact.Gender = characteristic.Gender;
            charact.Type = characteristic.Type;
            charact.Size = characteristic.Size;
            charact.SizeISS = characteristic.SizeISS;
            charact.SizeString = characteristic.SizeString;
            charact.Count = characteristic.Count;
            charact.DateOfAppearance = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Content("Произошла ошибка во время сохранения!");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public IActionResult CreateCharacteristic(int? id)
        {
            if (id == null)
                return NotFound();

            TempData["PrevPage"] = Request.Headers["Referer"].ToString();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCharacteristic(int? id, [Bind("Article,Brand,Brunch,Color,FullName,Gender,Size,SizeISS,SizeString,Type")] Characteristic characteristic)
        {
            if (id == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == id);
                characteristic.Product = product;
                characteristic.DateOfAppearance = DateTime.Now;

                await _context.Characteristics.AddAsync(characteristic);
                await _context.SaveChangesAsync();

                return Redirect(TempData["PrevPage"].ToString());
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCharacteristic(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            var characteristic = await _context.Characteristics
                .FirstOrDefaultAsync(item => item.ID == id);

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Ошибка при удалении. Попробуйте снова или обратитесь к системному администратору.";
            }

            TempData["PrevPage"] = Request.Headers["Referer"].ToString();

            return View(characteristic);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCharacteristic(int id, Characteristic characteristic)
        {
            if (characteristic == null)
                return RedirectToAction(nameof(DeleteCharacteristic), new { id, saveChangesError = true });

            try
            {
                _context.Characteristics.Remove(characteristic);
                await _context.SaveChangesAsync();
                return Redirect(TempData["PrevPage"].ToString());
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(DeleteCharacteristic), new { id, saveChangesError = true });
            }
        }

        private float Count(IEnumerable<Characteristic> characteristics)
        {
            float result = 0;

            foreach (var item in characteristics)
            {
                if(item.Count != null)
                    result += (float)item.Count;
            }

            return result;
        }
    }
}