using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Dtos;
using UserManagement.API.Entity;
using UserManagement.API.Services.Base;

namespace UserManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserManagementController : ControllerBase
{
    private readonly IUserService _userService;

    public UserManagementController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet(Name = "GetAll")]
    public async Task<IEnumerable<ApplicationUser>> Get()
    {
        var userList = await _userService.Get();
        return userList;
    }

    [HttpGet("{id}")]
    public async Task<ApplicationUser> GetById(string id)
    {
        var user = await _userService.GetById(id);
        if (user == null)
        {
            throw new Exception($"Could not found user");
        }
        return user;
    }

    [HttpPost(Name = "CreateUser")]
    public async Task<UserDto> Create(UserDto applicationUser)
    {
        if (ModelState.IsValid)
        {
            await _userService.Create(applicationUser);
        }
        return applicationUser;
    }

    [HttpPut("{id}")]
    public async Task<UserDto> Update(UserDto applicationUser, string id)
    {
        await _userService.Update(applicationUser, id);
        return applicationUser;
    }

    [HttpDelete("{id}")]
    public async Task<ApplicationUser> Delete(string id)
    {
        var user = await _userService.Delete(id);
        if (user == null)
        {
            throw new Exception($"Could not found user");
        }
        return user;
    }
}