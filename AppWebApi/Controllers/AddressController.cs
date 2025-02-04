using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppWebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAttractionService _attractionService;
    private readonly ILogger<AddressController> _logger;

    public AddressController(IAttractionService attractionService, ILogger<AddressController> logger)
    {
        _attractionService = attractionService;
        _logger = logger;
    }

    [HttpGet()]
    [ProducesResponseType(200, Type = typeof(List<IAddress>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> Read(bool seeded = true, bool flat = true, string filter = null,
        int pageNr = 0, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation($"{nameof(Read)}: {nameof(seeded)}: {seeded}, " +
                $"{nameof(pageNr)}: {pageNr}, {nameof(pageSize)}: {pageSize}");

            var addresses = await _attractionService.ReadAddressesAsync(seeded, flat, filter?.Trim().ToLower(), pageNr, pageSize);
            return Ok(addresses);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(Read)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(IAddress))]
    public async Task<IActionResult> ReadItem(string id = null, bool flat = false)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            _logger.LogInformation($"{nameof(ReadItem)}");

            var address = await _attractionService.ReadAddressAsync(guidId, flat);
            if (address?.Item == null) throw new ArgumentException($"Item with id {id} does not exist");
            return Ok(address);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(ReadItem)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAddress>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> DeleteItem(string id)
    {
        try
        {
            var idArg = Guid.Parse(id);

            _logger.LogInformation($"{nameof(DeleteItem)}: {nameof(idArg)}: {idArg}");
            
            var item = await _attractionService.DeleteAddressAsync(idArg);
            if (item?.Item == null) throw new ArgumentException($"Item with id {id} does not exist");
    
            _logger.LogInformation($"item {idArg} deleted");
            return Ok(item);                
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteItem)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAddress>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> UpdateItem(string id, [FromBody] AddressCuDto item)
    {
        try
        {
            var idArg = Guid.Parse(id);

            _logger.LogInformation($"{nameof(UpdateItem)}: {nameof(idArg)}: {idArg}");
            
            if (item.AddressId != idArg) throw new ArgumentException("Id mismatch");

            var _item = await _attractionService.UpdateAddressAsync(item);
            _logger.LogInformation($"item {idArg} updated");
           
            return Ok(_item);             
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateItem)}: {ex.Message}");
            return BadRequest($"Could not update. Error {ex.Message}");
        }
    }

    [HttpPost()]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAddress>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> CreateItem([FromBody] AddressCuDto item)
    {
        try
        {
            _logger.LogInformation($"{nameof(CreateItem)}:");

            var _item = await _attractionService.CreateAddressAsync(item);
            _logger.LogInformation($"item {_item.Item.AddressId} created");

            return Ok(_item);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(CreateItem)}: {ex.Message}");
            return BadRequest($"Could not create. Error {ex.Message}");
        }
    }

}