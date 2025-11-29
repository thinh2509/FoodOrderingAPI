using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrdering.Core.Enums
{
    public enum OrderStatus
    {
        Pending = 0,    // Mới đặt, chờ nhà hàng nhận
        Confirmed = 1,  // Nhà hàng đã nhận
        Cooking = 2,    // Đang nấu
        Delivering = 3, // Shipper đang giao
        Completed = 4,  // Giao thành công
        Cancelled = 5   // Đã hủy
    }
}
