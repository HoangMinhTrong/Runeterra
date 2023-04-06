using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Product.API.Data;
using Product.API.Dtos;
using Product.API.Dtos.Responses;
using Product.API.Entity;
using Product.API.Services.Base;

namespace Product.API.Services;

public class StoreService : IStoreService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;

    public StoreService(ApplicationDbContext context,  IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
    public async Task<bool> Create(CreateStoreRequest storeRequest)
    {
        if (storeRequest == null) return true;
        var store = new Store
        {
            Id = storeRequest.Id,
            Name = storeRequest.Name,
            ImageUrl = storeRequest.ImageUrl,
            Status = storeRequest.Status,
            Description = storeRequest.Description,
            UserId = GetUserId(storeRequest.UserId) 
        };
        await _context.AddAsync(store);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<StoreInfoResponse> GetStore(string? userId)
    {
        var store = await _context.Stores.SingleOrDefaultAsync(x => x.UserId == GetUserId(userId));
        if (store.IsActive)
        {
            var storeInfo = new StoreInfoResponse()
            {
                Id = store.Id,
                Name = store.Name,
                ImageUrl = store.ImageUrl,
                Status = store.Status,
                Description = store.Description,
                UserId = store.UserId
            };
            return storeInfo;
        }
        return null;
    }

    public string GetUserId(string userId)
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return userId = userIdClaim;
    }

    public async Task<bool> ActiveStore(int id)
    {
        var storeId = await _context.Stores.FindAsync(id);
        if (storeId != null)
        { 
            storeId.IsActive = true;
            _context.Stores.Update(storeId);
            await _context.SaveChangesAsync();
        }

        return true;
    }
}