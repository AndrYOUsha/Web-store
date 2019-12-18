using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Models;
using WebStore.Models.ProductViewModel;
using WebStore.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        public HomeController(ProductContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string something)
        {
            Response.Cookies.Append("Hello", "Hello, World!");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Products()
        {
            var viewModel = new ProductCharacteristicViewModel();

            viewModel.Products = await _context.Products
                .Include(p => p.Characteristic)
                .AsNoTracking()
                .ToListAsync();

            viewModel.PathToRoot = _appEnvironment.WebRootPath;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Product(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Characteristic)
                .FirstOrDefaultAsync(p => p.ID == id);

            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult WorkWithSession(string id)
        {
            if (HttpContext.Session.Keys.Contains(id))
                HttpContext.Session.Remove(id);
            else
                HttpContext.Session.SetString(id, id);

            int count = HttpContext.Session.Keys.Count();

            return Ok(count);
        }
    }
}
