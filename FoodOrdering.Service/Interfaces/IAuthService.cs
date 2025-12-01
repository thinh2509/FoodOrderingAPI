using FoodOrdering.Core.Wrappers;
using FoodOrdering.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrdering.Service.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> RegisterAsync(RegisterDto registerDto);
        Task<ServiceResponse<string>> LoginAsync(LoginDto loginDto);
    }
}
