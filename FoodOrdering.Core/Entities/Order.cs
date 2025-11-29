using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodOrdering.Core.Enums;

namespace FoodOrdering.Core.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid RestaurantId { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

        [Column(TypeName = "decimal(18,2)")] // Định dạng tiền tệ trong SQL
        public decimal TotalPrice { get; set; }

        [MaxLength(500)]
        public string ShippingAddress { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Note { get; set; } // Ghi chú: Ít cay, nhiều hành...

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Navigation
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        [ForeignKey("RestaurantId")]
        public virtual Restaurant? Restaurant { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
