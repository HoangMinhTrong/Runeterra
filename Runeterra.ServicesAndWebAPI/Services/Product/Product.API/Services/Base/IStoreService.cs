using Product.API.Dtos;
using Product.API.Dtos.Responses;
using Product.API.Entity;

namespace Product.API.Services.Base;

public interface IStoreService
{
    public Task<bool> Create(CreateStoreRequest store);
    public Task<StoreInfoResponse> GetStore(string userId);
    public string GetUserId(string userId);
}