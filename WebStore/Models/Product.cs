using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    /// <summary>
    /// Класс, в котором содертся вся основная информация о товаре
    /// Название, описание, размер скидки, цена, путь к папке с картинками, характеристики
    /// </summary>
    public class Product
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Описание модели")]
        public string Description { get; set; }

        [Display(Name = "Скидка")]
        public float? Discount { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Цена")]
        public decimal? Price { get; set; }

        public string PathToImages { get; set; }

        [Display(Name = "Характеристика товара")]
        public ICollection<Characteristic> Characteristic { get; set; }
    }
}
