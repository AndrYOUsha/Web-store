using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models.Identity.IdentityViewModels
{
    public class RecoveryModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Введите ваш Email")]
        public string Email { get; set; }
    }
}
