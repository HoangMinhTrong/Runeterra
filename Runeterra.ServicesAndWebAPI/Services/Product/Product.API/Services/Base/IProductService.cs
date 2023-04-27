using Product.API.Dtos;

namespace Product.API.Services.Base;

public interface IProductService
{
    public Task<List<Entity.Product>> Get();
    public Task<bool> Create(CreateProductRequest productDto);
    public Task<Entity.Product> GetById(int id);
}