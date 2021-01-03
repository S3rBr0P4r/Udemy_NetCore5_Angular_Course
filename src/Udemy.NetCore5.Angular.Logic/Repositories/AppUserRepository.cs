using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Udemy.NetCore5.Angular.Data;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Interfaces;

namespace Udemy.NetCore5.Angular.Logic.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AppUserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllChangesAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<IEnumerable<AppUserResponse>> GetUsersAsync()
        {
            return await _context.Users
                .ProjectTo<AppUserResponse>(_mapper.ConfigurationProvider)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<AppUserResponse> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .ProjectTo<AppUserResponse>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<AppUserResponse> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                .Where(u => u.UserName == userName)
                .ProjectTo<AppUserResponse>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync().ConfigureAwait(false);
        }
    }
}