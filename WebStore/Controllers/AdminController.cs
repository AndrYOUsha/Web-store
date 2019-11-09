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
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.AddRange(product);
                await _context.SaveChangesAsync();

                return Redirect(nameof(Index));
            }

            return View();
        }

        public async Task<IActionResult> Characteristics(int id)
        {
            var viewModel = new ProductCharacteristicViewModel();

            viewModel.Characteristics = await _context.Characteristics
                .Include(p => p.Product)
                .Where(p => p.Product.ID == id)
                .AsNoTracking()
                .ToListAsync();

            viewModel.Count = Count(viewModel.Characteristics);

            var product = viewModel.Characteristics.Where(p => p.Product.ID == id).FirstOrDefault();
            ViewData["TitleCharacteristics"] = $"Таблица характеристик товара \"{product.Product.Title}\"";

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateCharacteristic(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCharacteristic(int id, Characteristic characteristic)
        {
            if (ModelState.IsValid)
            {
                var product = _context.Products.Find(id);
                characteristic.ProductId = product.ID;
                characteristic.Product = product;
                _context.Add(characteristic);
                await _context.SaveChangesAsync();

                return Redirect(nameof(Index));
            }
            return View(characteristic);
        }

        private float Count(IEnumerable<Characteristic> count)
        {
            float result = 0;

            foreach(var c in count)
            {
                if(c.Count != null)
                    result += (float)c.Count;
            }

            return result;
        }
    }
}