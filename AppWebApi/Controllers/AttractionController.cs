﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Models.DTO;
using Services;
using Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers;
[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Policy = null, Roles = "usr, supusr, sysadmin")]
[ApiController]
[Route("api/[controller]/[action]")]
public class AttractionController : Controller
{
    readonly IAttractionService _attractionService;
    readonly ILogger<AttractionController> _logger;

    public AttractionController(IAttractionService attractionService, ILogger<AttractionController> logger)
    {
        _attractionService = attractionService;
        _logger = logger;
    }

    [HttpGet()]
    [ProducesResponseType(200, Type = typeof(List<IAttraction>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> Read(bool seeded = true, bool flat = true, string filter = null,
        int pageNr = 0, int pageSize = 10)
    {
        try
        {

            _logger.LogInformation($"{nameof(Read)}: {nameof(seeded)}: {seeded}, " +
                $"{nameof(pageNr)}: {pageNr}, {nameof(pageSize)}: {pageSize}");
            
            var attractions = await _attractionService.ReadAttractionsAsync(seeded, flat, filter?.Trim().ToLower(), pageNr, pageSize);
            return Ok(attractions);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(Read)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

     [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
            Policy = null, Roles = "usr,supusr, sysadmin")]
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAttraction>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> ReadItem(string id = null, bool flat = false)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            _logger.LogInformation($"{nameof(Read)}");

            var attraction = await _attractionService.ReadAttractionAsync(guidId, flat);
            if (attraction?.Item == null) throw new ArgumentException($"Item with id {id} does not exist");
            return Ok(attraction);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(Read)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Policy = null, Roles = "sysadmin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAttraction>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> DeleteItem(string id)
    {
        try
        {
            var idArg = Guid.Parse(id);

            _logger.LogInformation($"{nameof(DeleteItem)}: {nameof(idArg)}: {idArg}");
            
            var item = await _attractionService.DeleteAttractionAsync(idArg);
            if (item?.Item == null) throw new ArgumentException ($"Item with id {id} does not exist");
    
            _logger.LogInformation($"item {idArg} deleted");
            return Ok(item);                
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteItem)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }



    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Policy = null, Roles = "usr,supusr, sysadmin")]
    [HttpPut("{id}")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAttraction>))]
    [ProducesResponseType(400, Type = typeof(string))]    
    public async Task<IActionResult> UpdateItem(string id, [FromBody] AttractionCuDto item)
    {
        try
        {
            var idArg = Guid.Parse(id);

            _logger.LogInformation($"{nameof(UpdateItem)}: {nameof(idArg)}: {idArg}");
            
            if (item.AttractionId != idArg) throw new ArgumentException("Id mismatch");

            var _item = await _attractionService.UpdateAttractionAsync(item);
            _logger.LogInformation($"item {idArg} updated");
           
            return Ok(_item);             
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateItem)}: {ex.Message}");
            return BadRequest($"Could not update. Error {ex.Message}");
        }
    }

    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Policy = null, Roles = "usr, supusr, sysadmin")]
    [HttpPost()]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAttraction>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> CreateItem([FromBody] AttractionCuDto item)
    {
        try
        {
            _logger.LogInformation($"{nameof(CreateItem)}:");

            var _item = await _attractionService.CreateAttractionAsync(item);
            _logger.LogInformation($"item {_item.Item.AttractionId} created");

            return Ok(_item);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(CreateItem)}: {ex.InnerException?.Message}");
            return BadRequest($"Could not create. Error {ex.InnerException?.Message}");
        }
    }

}




