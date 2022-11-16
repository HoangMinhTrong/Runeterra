using Product.API.Dtos;
using Product.API.Entity;

namespace Product.API.Services.Base;

public interface IStoreService
{
    public Task<StoreDto> Create(StoreDto store);
}