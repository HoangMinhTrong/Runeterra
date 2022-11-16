using System.Net.Http.Headers;
using System.Security.Claims;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Product.API.Dtos;
using Product.API.Entity;
using Product.API.Services.Base;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoreController : ControllerBase
{

    private readonly IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    [HttpPost(Name = "CreateStore")]
    public async Task<StoreDto> Create(StoreDto store)
    {
        if (ModelState.IsValid)
        {
            await _storeService.Create(store);
        }
        return store;
    }

}