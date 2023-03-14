using System.Net.Http.Headers;
using System.Security.Claims;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Product.API.Dtos;
using Product.API.Dtos.Responses;
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
    public async Task<IActionResult> Create(CreateStoreRequest createStore)
    {
        if (ModelState.IsValid)
        {
            await _storeService.Create(createStore);
        }
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<StoreInfoResponse> GetStore(string? userId)
    {
        var store = await _storeService.GetStore(userId);
        return store;
    }

}