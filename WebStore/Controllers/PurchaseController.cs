using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using Microsoft.AspNetCore.Http;

namespace WebStore.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly ProductContext _contex;

        public PurchaseController(ProductContext contex)
        {
            _contex = contex;
        }

        [HttpGet]
        public async Task<IActionResult> Basket()
        {
            var products = await _contex.Products
                .AsNoTracking()
                .Where(p => HttpContext.Session.Keys.Contains(p.ID.ToString()))
                .ToListAsync();

            return View(products);
        }
    }
}