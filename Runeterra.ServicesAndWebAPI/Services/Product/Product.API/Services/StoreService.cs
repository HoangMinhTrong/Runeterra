using System.Security.Claims;
using Product.API.Data;
using Product.API.Dtos;
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
    public async Task<StoreDto> Create(StoreDto storeDto)
    {
        if (storeDto != null)
        {
            var store = new Store
            {
                Id = storeDto.Id,
                Name = storeDto.Name,
                ImageUrl = storeDto.ImageUrl,
                Status = storeDto.Status,
                Description = storeDto.Description,
                UserId = storeDto.UserId 
            };
            await _context.AddAsync(store);
            await _context.SaveChangesAsync();
        }
        return storeDto;
    }
}