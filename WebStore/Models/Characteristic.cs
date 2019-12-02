using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    /// <summary>
    /// Класс, в котором содержится вся дополнительная информация о товаре
    /// </summary>
    public class Characteristic : Item
    {
        public int ID { get; set; }

        [Display(Name = "Тип")]
        public string Type { get; set; }

        [Display(Name = "Брэнд")]
        public string Brand { get; set; }

        public string Brunch { get; set; }

        [Display(Name = "Название")]
        public string FullName { get; set; }

        [Display(Name = "Артикль")]
        public int? Article { get; set; }

        public float? Size { get; set; }

        public string SizeString { get; set; }

        [Display(Name = "Пол")]
        public Gender? Gender { get; set; }
        public InternationalSizingSystem? SizeISS { get; set; }
        [Display(Name = "Цвет")]
        public string Color { get; set; }

        [Display(Name = "Количество")]
        public float? Count { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата")]
        public DateTime DateOfAppearance { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Unisex
    }

    public enum InternationalSizingSystem
    {
        XS,
        S,
        M,
        L,
        XL,
        XXL,
        XXXL
    }
}
