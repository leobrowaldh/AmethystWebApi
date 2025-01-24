using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(bool seeded = true, string filter = null,
            int pageNr = 0, int pageSize = 10)
        {
            try
            {

                _logger.LogInformation($"{nameof(Read)}: {nameof(seeded)}: {seeded}, " +
                    $"{nameof(pageNr)}: {pageNr}, {nameof(pageSize)}: {pageSize}");
                
                var attractions = await _attractionService.ReadAsync(seeded, filter?.Trim().ToLower(), pageNr, pageSize);
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
        public async Task<IActionResult> ReadItem(string id = null)
        {
            try
            {
                Guid guidId = Guid.Parse(id);
                _logger.LogInformation($"{nameof(Read)}");

                var attraction = await _attractionService.ReadItemAsync(guidId);
                if (attraction?.Item == null) throw new ArgumentException($"Item with id {id} does not exist");
                return Ok(attraction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Read)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<AttractionModel>))]
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



          [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAttractionModel>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] AttractionCuDto item)
        {
            try
            {
                var idArg = Guid.Parse(id);

                _logger.LogInformation($"{nameof(UpdateItem)}: {nameof(idArg)}: {idArg}");
                
                if (item.Id != idArg) throw new ArgumentException("Id mismatch");

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

    }


    
}

