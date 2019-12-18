using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.AdditionalLogic;

namespace WebStore.Models.ProductViewModel
{
    public class ProductCharacteristicViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Characteristic> Characteristics { get; set; }
        public string PathToRoot { get; set; }
        public float Count { get; set; }
    }
}
