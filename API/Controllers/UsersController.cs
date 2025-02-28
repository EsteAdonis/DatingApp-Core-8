using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository) : BaseApiController
{
  [HttpGet]
	public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
	{
		var users = await userRepository.GetUserAsync();
		return Ok(users);
	}
	
	[HttpGet("{username}")]
	public async Task<ActionResult<AppUser>> GetUser(string username)
	{
		var user = await userRepository.GetUserByUsernameAsyunc(username);
		if (user == null) return NotFound();
		
		return user;
	}
}

