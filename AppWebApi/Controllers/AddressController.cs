using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IAddressService addressService, ILogger<AddressController> logger)
        {
            _addressService = addressService;
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

                var addresses = await _addressService.ReadAsync(seeded, flat, filter?.Trim().ToLower(), pageNr, pageSize);
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Read)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(IAddressModel))]
        public async Task<IActionResult> ReadItem(string id = null, bool flat = false)
        {
            try
            {
                Guid guidId = Guid.Parse(id);
                _logger.LogInformation($"{nameof(ReadItem)}");

                var address = await _addressService.ReadAddressAsync(guidId, flat);
                if (address?.Item == null) throw new ArgumentException($"Item with id {id} does not exist");
                return Ok(address);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItem)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}