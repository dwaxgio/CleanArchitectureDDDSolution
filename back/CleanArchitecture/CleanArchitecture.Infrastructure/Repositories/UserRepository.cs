using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) => _context = context;

        public async Task<List<User>> GetUsersAsync() => await _context.Users.ToListAsync();
        
        public async Task UpsertUsersAsync(List<User> users)
        {
            foreach(var user in users)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser == null)
                    _context.Users.Add(user);
                else
                {
                    existingUser.Name = user.Name;
                    existingUser.Country = user.Country;
                    existingUser.City = user.City;
                    existingUser.PictureUrl = user.PictureUrl;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
