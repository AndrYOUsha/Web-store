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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        ProductContext _context;
        IHostingEnvironment _appEnvironment;

        public AdminController(ProductContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ProductCharacteristicViewModel();

            viewModel.Products = await _context.Products
                .Include(p => p.Characteristic)
                .AsNoTracking()
                .ToListAsync();

            viewModel.PathToRoot = _appEnvironment.WebRootPath;

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
        public async Task<IActionResult> Create(Product product, IFormFileCollection uploadedFiles)
        {
            if (!ModelState.IsValid)
                return View();
            product.DateOfAppearances = DateTime.Now;

            #region Upload files
            if (uploadedFiles.Any())
            {
                int index = 0;
                var random = new Random();
                string nameFolder = $"{product.Title}_{random.Next(10000, 99999)}";
                string path = $"\\files\\images\\{nameFolder}";
                string fullPath = $"{_appEnvironment.WebRootPath}{path}";

                foreach (var file in uploadedFiles)
                {
                    if (file != null)
                    {
                        string tempPath = path;

                        if (!Directory.Exists(fullPath))
                            Directory.CreateDirectory(fullPath);

                        tempPath += $"\\{index++}_{file.FileName}";

                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + tempPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }

                product.PathToImages = path;

                await _context.AddAsync(product);
                await _context.SaveChangesAsync();

            }
            #endregion


            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
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
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.Characteristic)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ID == id);

            if (product.PathToImages != null)
                Directory.Delete($"{_appEnvironment.WebRootPath}{product.PathToImages}", true);

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
                if (item.Count != null)
                    result += (float)item.Count;
            }

            return result;
        }
    }
}