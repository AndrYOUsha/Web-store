using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;
using WebStore.Patterns.Interfaces;

namespace WebStore.Patterns.StrategyPattern
{
    public class AddItemToDB : IStrategy<Task<int>, object>
    {
        DbContext _context;
        Item _item;

        public AddItemToDB(DbContext context, Item item)
        {
            _context = context;
            _item = item;
        }

        public async Task<int> Add()
        {
            await _context.AddAsync(_item);
            return await _context.SaveChangesAsync();
        }

        public Task<int> Get(object none) { return null; }
    }
}
