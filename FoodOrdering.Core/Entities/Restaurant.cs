using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrdering.Core.Entities
{
    public class Restaurant : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Address { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsOpen { get; set; } = true;
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
