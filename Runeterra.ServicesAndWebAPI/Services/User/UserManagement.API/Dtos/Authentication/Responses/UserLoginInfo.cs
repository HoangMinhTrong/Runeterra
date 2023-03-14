using System.ComponentModel.DataAnnotations;

namespace UserManagement.API.Dtos.Authentication.Responses;

public class UserLoginInfo
{
    [Required(ErrorMessage = "Username is required")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; init; }
}