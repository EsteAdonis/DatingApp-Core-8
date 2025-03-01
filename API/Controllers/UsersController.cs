using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;
using AutoMapper;
namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{
  [HttpGet]
	public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
	{
		var users = await userRepository.GetUserAsync();
		var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users);
		return Ok(usersToReturn);
	}
	
	[HttpGet("{username}")]
	public async Task<ActionResult<MemberDto>> GetUser(string username)
	{
		var user = await userRepository.GetUserByUsernameAsyunc(username);
		if (user == null) return NotFound();
		
		return mapper.Map<MemberDto>(user);
	}
}

