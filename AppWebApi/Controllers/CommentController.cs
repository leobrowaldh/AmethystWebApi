using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Models.DTO;
using Services;
using Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CommentController : Controller
{
    readonly IAttractionService _attractionService;
    readonly ILogger<CommentController> _logger;

    public CommentController(IAttractionService attractionService, ILogger<CommentController> logger)
    {
        _attractionService = attractionService;
        _logger = logger;
    }

    [HttpGet()]
    [ProducesResponseType(200, Type = typeof(List<IComment>))]
    [ProducesResponseType(400, Type = typeof(string))]
    public async Task<IActionResult> Read(bool seeded = true, bool flat = true, string filter = null,
        int pageNr = 0, int pageSize = 10)
    {
        try
        {

            _logger.LogInformation($"{nameof(Read)}: {nameof(seeded)}: {seeded}, " +
                $"{nameof(pageNr)}: {pageNr}, {nameof(pageSize)}: {pageSize}");
            
            var attractions = await _attractionService.ReadCommentsAsync(seeded, flat, filter?.Trim().ToLower(), pageNr, pageSize);
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
    public async Task<IActionResult> ReadItem(string id = null, bool flat = false)
    {
        try
        {
            Guid guidId = Guid.Parse(id);
            _logger.LogInformation($"{nameof(Read)}");

            var attraction = await _attractionService.ReadCommentAsync(guidId, flat);
            if (attraction?.Item == null) throw new ArgumentException($"Item with id {id} does not exist");
            return Ok(attraction);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(Read)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    
}




