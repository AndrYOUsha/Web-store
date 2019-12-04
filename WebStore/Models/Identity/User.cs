using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models.Identity
{
    public class User : Item
    {
        public int ID { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        public string Email { get; set; }

        public DateTime DateRegistered { get; set; }
        public string Phone { get; set; }
        public int? Age { get; set; }

        [Required(ErrorMessage = "Введите Password")]
        [MinLength(8)]
        [MaxLength(20)]
        public string Password { get; set; }

        public bool ConfirmEmail { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
