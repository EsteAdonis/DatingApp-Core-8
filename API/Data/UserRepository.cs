using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<AppUser>> GetUserAsync()
    {
      return await context
                  .Users
                  .Include(x => x.Photos)
                  .ToListAsync();
    }

    public Task<AppUser?> GetUserByUsernameAsyunc(string username)
    {
      return context
            .Users
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<bool> SaveAllAsync()
    {
      return await context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
      context.Entry(user).State = EntityState.Modified;
    }

    public async Task<IEnumerable<MemberDto>> GetMemberAsync()
    {
      return await context.Users
                  .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                  .ToListAsync();
    }

    public async Task<MemberDto?> GetMemberAsync(string username)
    {
      return await context.Users  
                  .Where(x => x.UserName == username)
                  .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync();
    }
}