using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models.Identity
{
    public class Role : Item
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
