using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService, IMapper mapper): BaseApiController 
{
  [HttpPost("register")]  //account/register
  public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
  {
    if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");
    
    using var hmac = new HMACSHA512();
    var user= mapper.Map<AppUser>(registerDto);

    user.UserName = registerDto.Username.ToLower();
    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
    user.PasswordSalt = hmac.Key; 

    context.Users.Add(user);
    await context.SaveChangesAsync();

    return new UserDto {
      Username = user.UserName,
      KnownAs = user.KnownAs,      
      Token = tokenService.CreateToken(user),
      PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
      Gender = user.Gender
    };
  }

  [HttpPost("login")]
  public async Task<ActionResult<UserDto>>Login(LoginDto logingDto)
  {
    var user = await context
                    .Users
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(u => u.UserName.ToLower() == logingDto.Username.ToLower());
    
    if(user == null) return Unauthorized("Invalid username or password");

    using var hmac = new HMACSHA512(user.PasswordSalt);
    var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logingDto.Password));

    for(int i =0;  i < computerHash.Length; i++)
    {
      if (computerHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
    }

    return new UserDto {
      Username = user.UserName,
      Token = tokenService.CreateToken(user),
      PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
      KnownAs = user.KnownAs,
      Gender = user.Gender
    };
  }

  private async Task<bool> UserExists(string username)
  {
    if (string.IsNullOrEmpty(username))
    {
        return false;
    }
    return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
  }
}
