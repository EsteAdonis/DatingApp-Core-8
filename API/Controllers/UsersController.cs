using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;
using System.Security.Claims;
using AutoMapper;
using API.Extensions;
using API.Entities;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, 
													   IMapper mapper,
														 IPhotoService photoService) : BaseApiController
{
  [HttpGet]
	public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
	{
		var users = await userRepository.GetMemberAsync();
		return Ok(users);
	}
	
	[HttpGet("{username}")]
	public async Task<ActionResult<MemberDto>> GetUser(string username)
	{
		var user = await userRepository.GetMemberAsync(username);
		if (user == null) return NotFound();
		return user;
	}

	[HttpPut]
	public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
	{
		var user = await userRepository.GetUserByUsernameAsyunc(User.GetUsername());

		if (user == null) return BadRequest("Could not find user");
		
		mapper.Map(memberUpdateDto, user);

		if (await userRepository.SaveAllAsync() ) return NoContent();

		return BadRequest("Failed to update the user");
	}
	
	[HttpPost("add-photo")]
	public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
	{
		var user = await userRepository.GetUserByUsernameAsyunc(User.GetUsername());
		if (user == null) return BadRequest("Cannot update user");
		var result =  await photoService.AddPhotoAsync(file);
		if(result.Error != null) return BadRequest(result.Error.Message);
		
		var photo = new Photo 
		{
			Url = result.SecureUrl.AbsoluteUri,
			PublicId = result.PublicId,
		};

		user.Photos.Add(photo);

		if (await userRepository.SaveAllAsync()) return mapper.Map<PhotoDto>(photo);

		return BadRequest("Problem adding photo");
	}
}

