using API.DTOs;
using API.Entities;
using API.Helpers;
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

    public async Task<PagedList<MemberDto>> GetMemberAsync(UserParams userParams)
    {
      var query = context.Users.AsQueryable();

      query = query.Where(x => x.UserName != userParams.CurrentUsername);

      if(userParams.Gender != null)
      {
        query = query.Where(x => x.Gender == userParams.Gender);
      }

      var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge-1));
      var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

      query = query.Where(x=> x.DateOfBirth > minDob && x.DateOfBirth <= maxDob);

      return await PagedList<MemberDto>
                  .CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), 
                               userParams.PageNumber, 
                               userParams.PageSize);
    }

    public async Task<MemberDto?> GetMemberAsync(string username)
    {
      return await context.Users  
                  .Where(x => x.UserName == username)
                  .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                  .SingleOrDefaultAsync();
    }
}