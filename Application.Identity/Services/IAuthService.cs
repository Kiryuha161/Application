using Application.Identity.Data;
using Application.Identity.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(IdentityDbContext dbContext, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return AuthResult.Fail("Email уже существует");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Username = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                IsEmailConfirmed = false
            };

            var defaultRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User");

            if (defaultRole != null)
            {
                user.UserRoles.Add(new UserRole { RoleId = defaultRole.Id });
            }

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var roles = await GetUserRolesAsync(user.Id);
            var permissions = await GetUserPermissionsAsync(user.Id);
            var token = _tokenService.GenerateToken(user, roles, permissions);

            return AuthResult.Success(token, user);
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            var user = await _dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return AuthResult.Fail("Неверный email или пароль");
            }

            user.LastLoginAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            var roles = await GetUserRolesAsync(user.Id);
            var permissions = await GetUserPermissionsAsync(user.Id);
            var token = _tokenService.GenerateToken(user, roles, permissions);

            return AuthResult.Success(token, user);
        }

        private async Task<List<string>> GetUserRolesAsync(Guid userId)
        {
            return await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.Name)
                .ToListAsync();
        }

        private async Task<List<string>> GetUserPermissionsAsync(Guid userId)
        {
            return await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToListAsync();
        }
    };
}
