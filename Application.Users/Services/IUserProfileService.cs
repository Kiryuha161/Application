using Application.Users.Data;
using Application.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Services
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetUserProfileAsync(Guid userId);
        Task<UserProfile> CreateUserProfileAsync(Guid userId, string firstName, string lastName);
    }

    public class UserProfileService : IUserProfileService
    {
        private readonly UsersDbContext _dbContext;

        public UserProfileService(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProfile> GetUserProfileAsync(Guid userId)
        {
            return await _dbContext.Profiles.FindAsync(userId);
        }

        public async Task<UserProfile> CreateUserProfileAsync(Guid userId, string firstName, string lastName)
        {
            var profile = new UserProfile
            {
                UserId = userId,
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.AddAsync(profile);
            await _dbContext.SaveChangesAsync();

            return profile;
        }
    }
}
