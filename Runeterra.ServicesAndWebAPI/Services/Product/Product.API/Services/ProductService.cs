using System.Net.Http.Headers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Product.API.Data;
using Product.API.Dtos;
using Product.API.Entity;
using Product.API.Services.Base;

namespace Product.API.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
        
    }
    public async Task<List<Entity.Product>> Get()
    {
        var products = await _context.Products
            .Include(x => x.ProductType)
            .Include(x => x.Store)
            .ToListAsync();
        return products;
    }

    public async Task<bool> Create(CreateProductRequest productDto)
    {
        if (productDto != null)
        {
            var product = new Entity.Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Status = productDto.Status,
                Quantity = productDto.Quantity,
                ProductTypeId = productDto.ProductTypeId,
                StoreId = productDto.StoreId
            };
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        return true;
    }
}