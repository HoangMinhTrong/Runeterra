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
    private readonly IRequestClient<ApplicationUser> _client;
    public ProductService(ApplicationDbContext context, IRequestClient<ApplicationUser> client)
    {
        _context = context;
        _client = client;
    }
    public async Task<List<Entity.Product>> Get()
    {
        string baseUrl = "https://localhost:7241";
        var applicationUsers = new List<ApplicationUser>();
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var res = await client.GetAsync("/gateway/user");
            if (res.IsSuccessStatusCode)
            {
                var userResponse = res.Content.ReadAsStringAsync().Result;
                applicationUsers = JsonConvert.DeserializeObject<List<ApplicationUser>>(userResponse);
            }
        }
        var products = await _context.Products.Include(x => x.Image)
            .Include(x => x.ProductType)
            .Include(x => x.Store)
            .ToListAsync();
        return products;
    }

    public async Task<ProductDto> Create(ProductDto productDto)
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
                ImageId = productDto.ImageId,
                ProductTypeId = productDto.ProductTypeId,
                StoreId = productDto.StoreId
            };
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        return productDto;
    }
}