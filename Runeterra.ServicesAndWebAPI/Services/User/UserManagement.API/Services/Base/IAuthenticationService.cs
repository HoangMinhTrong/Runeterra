using UserManagement.API.Dtos.Authentication.Responses;

namespace UserManagement.API.Services.Base;

public interface IAuthenticationService
{
    Task<bool> ValidateUserAsync(UserLoginInfo loginDto); 
    Task<string> CreateTokenAsync(); 
}