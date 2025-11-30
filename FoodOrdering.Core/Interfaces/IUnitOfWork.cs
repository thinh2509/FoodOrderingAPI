using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrdering.Core.Interfaces
{
    public interface IUnitOfWork
    {
        // Khai báo các repo cụ thể
        Task<int> CompleteAsync();
    }
}
