using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Models.ProductViewModel;
using WebStore.Patterns.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Patterns.StrategyPattern
{
    public enum ItemSelectorPCVM
    {
        Product,
        Characteristic
    }

    public class GetViewModel : IStrategy<Task<ProductCharacteristicViewModel>, ItemSelectorPCVM>
    {
        ProductContext _context;
        int _id;

        public GetViewModel(ProductContext context, int? id)
        {
            _context = context;

            if(id != null)
                _id = (int)id;
        }

        public Task<ProductCharacteristicViewModel> Add() { return null; }

        public async Task<ProductCharacteristicViewModel> Get(ItemSelectorPCVM selector)
        {
            var viewModel = new ProductCharacteristicViewModel();

            switch (selector)
            {
                case ItemSelectorPCVM.Product:

                    viewModel.Products = await _context.Products
                        .Include(c => c.Characteristic)
                        .AsNoTracking()
                        .ToListAsync();
                    break;

                case ItemSelectorPCVM.Characteristic:

                    viewModel.Characteristics = await _context.Characteristics
                        .Where(p => p.Product.ID == _id)
                        .Include(p => p.Product)
                        .AsNoTracking()
                        .ToListAsync();

                    if (viewModel.Characteristics.Any())
                        viewModel.Count = Count(viewModel.Characteristics);
                    break;

                default:
                    break;
            }

            return viewModel;
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
