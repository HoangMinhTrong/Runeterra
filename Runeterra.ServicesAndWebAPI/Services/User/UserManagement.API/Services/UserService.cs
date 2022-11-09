using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.Dtos;
using UserManagement.API.Entity;
using UserManagement.API.Services.Base;

namespace UserManagement.API.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<ApplicationUser>> Get()
    {
        var userList = await _context.Users.ToListAsync();
        await _context.SaveChangesAsync();
        return userList;
    }

    public async Task<ApplicationUser> GetById(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<UserDto> Create(UserDto applicationUser)
    {
        // var user = new ApplicationUser();
        if (applicationUser != null)
        {
            var user = new ApplicationUser
            {
                Id = applicationUser.UserId,
                UserName = applicationUser.UserName,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                BuildingNo = applicationUser.BuildingNo,
                Role = applicationUser.Roles,
                Email = applicationUser.Email
            };
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        return applicationUser;
    }

    public async Task<UserDto> Update(UserDto applicationUser, string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        user.Id = applicationUser.UserId;
        user.UserName = applicationUser.UserName;
        user.FirstName = applicationUser.FirstName;
        user.LastName = applicationUser.LastName;
        user.BuildingNo = applicationUser.BuildingNo;
        user.Role = applicationUser.Roles;
        user.Email = applicationUser.Email;
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return applicationUser;
    }

    public async Task<ApplicationUser> Delete(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        _context.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }
}