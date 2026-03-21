using Application.Abstractions.Common;
using Application.Abstractions.DTO.Users;
using Application.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Services
{
    public interface IIdentityUserService
    {
        Task<ApiResult<UserDto?>> GetByIdAsync(Guid id);
        Task<ApiResult<List<UserDto>>> GetAllAsync();
    }

    public class IdentityUserService : IIdentityUserService
    {
        private readonly IdentityDbContext _dbContext;

        public IdentityUserService(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResult<List<UserDto?>>> GetAllAsync()
        {
            var users = await _dbContext.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email
            }).ToListAsync();

            return ApiResult<List<UserDto>>.Success(users);
        }

        public async Task<ApiResult<UserDto?>> GetByIdAsync(Guid id)
        {
            var user = await _dbContext.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email
            }).FirstOrDefaultAsync();

            return ApiResult<UserDto>.Success(user);
        }
    }
}
