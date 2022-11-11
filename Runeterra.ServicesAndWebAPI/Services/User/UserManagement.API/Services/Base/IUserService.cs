using UserManagement.API.Dtos;
using UserManagement.API.Entity;

namespace UserManagement.API.Services.Base;

public interface IUserService
{
    public Task<IEnumerable<ApplicationUser>> Get();
    public Task<ApplicationUser> GetById(string id);
    public Task<UserDto> Create(UserDto applicationUser);
    public Task<UserDto> Update(UserDto applicationUser, string id);
    public Task<ApplicationUser> Delete(string id);
}