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

namespace WebStore.Controllers
{
    public class AdminController : Controller
    {
        ProductContext _context;

        public AdminController(ProductContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productViewModel = new ProductCharacteristicViewModel();


            productViewModel.Products = await _context.Products
                .Include(i => i.Characteristic)
                .AsNoTracking()
                .ToListAsync();

            return View(productViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Discount,Price")]Product product)
        {
            if (!ModelState.IsValid)
                return View();

            _context.AddRange(product);
            await _context.SaveChangesAsync();

            return Redirect(nameof(Index));
        }

        public async Task<IActionResult> Characteristics(int? id)
        {
            if (id == null)
                return NotFound();


            var viewModel = new ProductCharacteristicViewModel();

            viewModel.Characteristics = await _context.Characteristics
                .Where(p => p.Product.ID == id)
                .Include(p => p.Product)
                .AsNoTracking()
                .ToListAsync();

            if (!viewModel.Characteristics.Any())
                return NotFound();

            viewModel.Count = Count(viewModel.Characteristics);

            var product = viewModel.Characteristics.Where(p => p.Product.ID == id).FirstOrDefault().Product;
            ViewData["TitleCharacteristics"] = $"Таблица характеристик товара \"{product.Title}\"";

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateCharacteristic(int? id)
        {
            if (id == null)
                return NotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                await _context.Characteristics.AddAsync(characteristic);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private float Count(IEnumerable<Characteristic> count)
        {
            float result = 0;

            if (count != null)
            {
                foreach (var c in count)
                {
                    if (c.Count != null)
                        result += (float)c.Count;
                }
            }
            return result;
        }
    }
}