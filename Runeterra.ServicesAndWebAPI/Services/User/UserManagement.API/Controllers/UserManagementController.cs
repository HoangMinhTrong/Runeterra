using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Dtos;
using UserManagement.API.Dtos.Authentication.Responses;
using UserManagement.API.Entity;
using UserManagement.API.Services.Base;
using IAuthenticationService = UserManagement.API.Services.Base.IAuthenticationService;

namespace UserManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserManagementController : ControllerBase
{
    private readonly IAuthenticationService _authenService;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserManagementController(IUserService userService, IAuthenticationService authenService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _authenService = authenService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpGet(Name = "GetAll")]
    [Authorize]
    public async Task<IEnumerable<ApplicationUser>> Get()
    {
        var userList = await _userService.Get();
        // Get the user's Id from the claims
        var userIdClaim= _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = userIdClaim.Value;
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
    public async Task<UserInfoResponse> Create(UserInfoResponse applicationUser)
    {
        if (ModelState.IsValid)
        {
            await _userService.Create(applicationUser);
        }
        return applicationUser;
    }

    [HttpPut("{id}")]
    public async Task<UserInfoResponse> Update(UserInfoResponse applicationUser, string id)
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
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] UserLoginInfo user)
    {
        return !await _authenService.ValidateUserAsync(user)
            ? Unauthorized() 
            : Ok(new { Token = await _authenService.CreateTokenAsync() });
    }
}