
using System.Security.Claims;
using Identity.API.Entities;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services;

public class ProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly RoleManager<IdentityRole> _roleMgr;

    public ProfileService(
        UserManager<ApplicationUser> userMgr,
        RoleManager<IdentityRole> roleMgr,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
    {
        _userMgr = userMgr;
        _roleMgr = roleMgr;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
    }
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        string sub = context.Subject.GetSubjectId();
        ApplicationUser user = await _userMgr.FindByIdAsync(sub);
        ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);


        List<Claim> claims = userClaims.Claims.ToList();
        claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
        claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
        claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
        if (_userMgr.SupportsUserRole)
        {
            IList<string> roles = await _userMgr.GetRolesAsync(user);
            foreach(var rolename in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, rolename));
                if (_roleMgr.SupportsRoleClaims)
                {
                    IdentityRole role = await _roleMgr.FindByNameAsync(rolename);
                    if (role != null)
                    {
                        claims.AddRange(await _roleMgr.GetClaimsAsync(role));
                    }
                }
            }
        }

        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        string sub = context.Subject.GetSubjectId();
        ApplicationUser user = await _userMgr.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}