using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.API.Dtos;
using UserManagement.API.Entity;
using UserManagement.API.Services.Base;

namespace UserManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserManagementController : ControllerBase
{
    private readonly UserManager <ApplicationUser> _userManager;
    private readonly SignInManager <ApplicationUser> _signInManager;
    private readonly IUserService _userService;

    public UserManagementController(IUserService userService,SignInManager <ApplicationUser> signInManager, UserManager <ApplicationUser> userManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
    
    [HttpPost]
    public async Task < IActionResult > SignIn(LoginInputModel signIn, string ReturnUrl) {
        ApplicationUser user;
        if (signIn.Username.Contains("@")) {
            user = await _userManager.FindByEmailAsync(signIn.Username);
        } else {
            user = await _userManager.FindByNameAsync(signIn.Username);
        }
        if (user == null) {
            ModelState.AddModelError("", "Login fail");
        }
        var result = await
            _signInManager.PasswordSignInAsync(user, signIn.Password, signIn.RememberLogin, true);
        if (!result.Succeeded) {
            ModelState.AddModelError("", "Login fail");
        }
        if (result.Succeeded)
        {
            return LocalRedirect(ReturnUrl);
        }
        if (ReturnUrl != null) return LocalRedirect(ReturnUrl);
        return Ok();
    }
    
    // [HttpPost(Name = "CreateUser")]
    // public async Task<UserDto> Create(UserDto applicationUser)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         await _userService.Create(applicationUser);
    //     }
    //     return applicationUser;
    // }

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