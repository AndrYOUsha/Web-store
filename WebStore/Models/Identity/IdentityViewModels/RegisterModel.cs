using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models.Identity.IdentityViewModels
{
    public class RegisterModel
    {
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        public string Email { get; set; }

        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }

        [Display(Name = "Возраст")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Должно быть не менее 8-ми знаков")]
        [MaxLength(20, ErrorMessage = "Должно быть не более 20-ти знаков")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [Compare("Password", ErrorMessage = "Пароль неверный")]
        [Display(Name = "Повторите пароль")]
        public string ComparePassword { get; set; }
    }
}
