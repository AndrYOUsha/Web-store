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

namespace WebStore.Controllers
{
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
            var productViewModel = await strategy.GetItemViewModelAsynk(new GetViewModel(_context, null), ItemSelectorPCVM.Product);

            if(User.Identity.IsAuthenticated)
                ViewData["Identity"] = User.Identity.Name;

            return View(productViewModel);
        }

        //Создать продукт и характристику
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
                return View();

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Characteristic)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ID == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.Characteristic)
                .SingleAsync(p => p.ID == id);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Characteristics(int? id)
        {
            if (id == null)
                return NotFound();

            var viewModel = await strategy.GetItemViewModelAsynk(new GetViewModel(_context, id), ItemSelectorPCVM.Characteristic);

            if (!viewModel.Characteristics.Any())
                return RedirectToAction(nameof(Index));

            var product = viewModel.Characteristics.Where(p => p.Product.ID == id).FirstOrDefault().Product;
            ViewData["TitleCharacteristics"] = $"Таблица характеристик товара \"{product.Title}\"";

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Characteristics(int id, Characteristic characteristic)
        {
            var charact = await _context.Characteristics
                .FirstOrDefaultAsync(c => c.ID == characteristic.ID);

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

            try
            {
                _context.Characteristics.Update(charact);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Content("Произошла ошибка во время сохранения!");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult CreateCharacteristic(int? id)
        {
            if (id == null)
                return NotFound();

            TempData["PrevPage"] = Request.Headers["Referer"].ToString();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCharacteristic(int? id, [Bind("Article, Brand, Brunch, Color, Count, " +
            "FullName, Gender, Size, SizeIIS, SizeString, Type")] Characteristic characteristic)
        {
            if (id == null)
                return NotFound();

            if ((characteristic.Article == null) && (characteristic.Brand == null) &&
                (characteristic.Brunch == null) && (characteristic.Color == null) &&
                (characteristic.Count == null) && (characteristic.FullName == null) &&
                (characteristic.Gender == null) && (characteristic.Size == null) &&
                (characteristic.SizeISS == null) && (characteristic.SizeString == null) &&
                (characteristic.Type == null)) return Content("Хотя бы одно поле должно быть заполнено.");


            if (ModelState.IsValid)
            {
                var product = await _context.Products.Where(p => p.ID == id).FirstAsync();
                characteristic.Product = product;

                await strategy.AddItemAsynk(new AddItemToDB(_context, characteristic));

                return Redirect(TempData["PrevPage"].ToString());
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCharacteristic(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            var characteristic = await _context.Characteristics
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ID == id);

            if (characteristic == null)
                return RedirectToAction(nameof(Index));

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Ошибка при удалении. Попробуйте снова или обратитесь к системному администратору.";
            }

            TempData["PrevPage"] = Request.Headers["Referer"].ToString();

            return View(characteristic);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCharacteristic(int id)
        {
            var characteristic = await _context.Characteristics.FindAsync(id);

            if (characteristic == null)
                return RedirectToAction(nameof(Index));

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
    }
}