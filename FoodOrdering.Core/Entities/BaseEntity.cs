using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrdering.Core.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? LastUpdatedTime { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeleteTime { get; set; } = DateTimeOffset.UtcNow;
    }
}
