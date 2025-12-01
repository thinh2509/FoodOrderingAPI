using AutoMapper;
using FoodOrdering.Core.Entities;
using FoodOrdering.Core.Wrappers;
using FoodOrdering.Service.DTOs;
using FoodOrdering.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace FoodOrdering.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> LoginAsync(LoginDto loginDto)
        {
            // Khởi tạo response
            var response = new ServiceResponse<string>();
            // Tìm user trong DB
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if(user == null)
            {
                response.Success = false;
                response.Message = "Email hoặc mật khẩu không chính xác";
                return response;
            }
            // Kiểm tra password
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                response.Success = false;
                response.Message = "Email hoặc mật khẩu không chính xác";
                return response;
            }
            // Login thành công tạo token
            // Tạm thời trả về id để test flow
            response.Data = GenerateJwtToken(user);
            response.Message = "Đăng nhập thành công";
            return response;
        }

        public async Task<ServiceResponse<string>> RegisterAsync(RegisterDto registerDto)
        {
            // Khởi tạo response
            var response = new ServiceResponse<string>();
            try
            {
                // Tìm email có tồn tại trong db không
                var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
                if(userExists != null)
                {
                    response.Success = false;
                    response.Message = "Email đã tồn tại";
                    return response;
                }
                var user = _mapper.Map<ApplicationUser>(registerDto);
                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (result.Succeeded)
                {
                    // Có thể gán Role mặc định là "Customer" ở đây (sẽ làm sau khi có Role)
                    response.Data = user.Id.ToString();
                    response.Message = "Đăng ký thành công!";
                }
                else
                {
                    response.Success = false;
                }
            } catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        private string GenerateJwtToken(ApplicationUser user)
        {
            // 1. Lấy cấu hình JwtSetiings
            var jwtSettings = _configuration.GetSection("JwtSettings");
            // 2. Lấy Key từ cấu hình JwtSettings chuỗi chuyển sang byte
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!); // null-forgiving operator: đảm bảo không null

            // 3. Tạo danh sách Claims chứa thông tin Token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.FullName),
            };
            // 4. Tạo Token Descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };
            // 5. Sinh ra Token bằng TokenHanlder
            var tokenHanlder = new JwtSecurityTokenHandler();
            var token = tokenHanlder.CreateToken(tokenDescriptor);
            return tokenHanlder.WriteToken(token);
        }
    }
}
