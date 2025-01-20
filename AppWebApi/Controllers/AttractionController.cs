﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Models.DTO;
using Services;
using Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AttractionController : Controller
    {
        readonly IAttractionService _attractionService;
        readonly ILogger<GuestController> _logger;

        public AttractionController(IAttractionService attractionService, ILogger<GuestController> logger)
        {
            _attractionService = attractionService;
            _logger = logger;
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(List<IAttractionModel>))]
        public async Task<IActionResult> Read()
        {
            try
            {
                var attractions = await _attractionService.ReadAsync();

                _logger.LogInformation($"{nameof(Read)}");
                return Ok(attractions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Read)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IAttractionModel))]
        public async Task<IActionResult> ReadItem(string id)
        {
            try
            {
                Guid guidId = Guid.Parse(id);
                var attraction = await _attractionService.ReadItemAsync(guidId);

                _logger.LogInformation($"{nameof(Read)}");
                return Ok(attraction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Read)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

