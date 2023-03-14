using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using UserManagement.API.Entity;
using IAuthenticationService = UserManagement.API.Services.Base.IAuthenticationService;
using UserLoginInfo = UserManagement.API.Dtos.Authentication.Responses.UserLoginInfo;

namespace UserManagement.API.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private ApplicationUser? _user;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<bool> ValidateUserAsync(UserLoginInfo loginDto)
    {
        _user = await _userManager.FindByNameAsync(loginDto.UserName);
        var result = _user != null && await _userManager.CheckPasswordAsync(_user, loginDto.Password);
        return result;
    }

    public async Task<string> CreateTokenAsync()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
    private SigningCredentials GetSigningCredentials()
    {
        var jwtConfig = _configuration.GetSection("jwtConfig");
        var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, _user.Id),
            new Claim(ClaimTypes.Name, _user.UserName)
        };
        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtConfig");
        var tokenOptions = new JwtSecurityToken
        (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresIn"])),
            signingCredentials: signingCredentials
        );
        return tokenOptions;
    }
}