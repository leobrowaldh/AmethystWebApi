using Microsoft.AspNetCore.Mvc;

using Models;
using Models.DTO;
using Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VisitorController : Controller
    {
        readonly IAttractionService _service = null;
        readonly ILogger<VisitorController> _logger = null;

        public VisitorController(IAttractionService service, ILogger<VisitorController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(ResponsePageDto<IVisitor>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItems(string seeded = "true", string flat = "true",
            string filter = null, string pageNr = "0", string pageSize = "10")
        {
            try
            {
                bool seededArg = bool.Parse(seeded);
                bool flatArg = bool.Parse(flat);
                int pageNrArg = int.Parse(pageNr);
                int pageSizeArg = int.Parse(pageSize);

                _logger.LogInformation($"{nameof(ReadItems)}: {nameof(seededArg)}: {seededArg}, {nameof(flatArg)}: {flatArg}, " +
                    $"{nameof(pageNrArg)}: {pageNrArg}, {nameof(pageSizeArg)}: {pageSizeArg}");
                
                var resp = await _service.ReadVisitorssAsync(seededArg, flatArg, filter?.Trim().ToLower(), pageNrArg, pageSizeArg);     
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItems)}: {ex.Message}.{ex.InnerException?.Message}");
                return BadRequest($"{ex.Message}.{ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<IVisitor>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItem(string id = null, string flat = "false")
        {
            try
            {
                var idArg = Guid.Parse(id);
                bool flatArg = bool.Parse(flat);

                _logger.LogInformation($"{nameof(ReadItem)}: {nameof(idArg)}: {idArg}, {nameof(flatArg)}: {flatArg}");
                
                var item = await _service.ReadVisitorAsync(idArg, flatArg);
                if (item?.Item == null) throw new ArgumentException ($"Item with id {id} does not exist");

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItem)}: {ex.Message}.{ex.InnerException?.Message}");
                return BadRequest($"{ex.Message}.{ex.InnerException?.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<IVisitor>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteItem(string id)
        {
            try
            {
                var idArg = Guid.Parse(id);

                _logger.LogInformation($"{nameof(DeleteItem)}: {nameof(idArg)}: {idArg}");
                
                var item = await _service.DeleteVisitorAsync(idArg);
                if (item?.Item == null) throw new ArgumentException ($"Item with id {id} does not exist");
        
                _logger.LogInformation($"item {idArg} deleted");
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DeleteItem)}: {ex.Message}.{ex.InnerException?.Message}");
                return BadRequest($"{ex.Message}.{ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<VisitorCuDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItemDto(string id = null)
        {
            try
            {
                var idArg = Guid.Parse(id);

                _logger.LogInformation($"{nameof(ReadItemDto)}: {nameof(idArg)}: {idArg}");

                var item = await _service.ReadVisitorAsync(idArg, false);
                if (item?.Item == null) throw new ArgumentException ($"Item with id {id} does not exist");

                return Ok(
                    new ResponseItemDto<VisitorCuDto>() {
                    DbConnectionKeyUsed = item.DbConnectionKeyUsed,
                    Item = new VisitorCuDto(item.Item)
                });   
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItemDto)}: {ex.Message}.{ex.InnerException?.Message}");
                return BadRequest($"{ex.Message}.{ex.InnerException?.Message}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<IVisitor>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] VisitorCuDto item)
        {
            try
            {
                var idArg = Guid.Parse(id);

                _logger.LogInformation($"{nameof(UpdateItem)}: {nameof(idArg)}: {idArg}");
                
                if (item.EmployeeId != idArg) throw new ArgumentException("Id mismatch");

                var model = await _service.UpdateEmployeeAsync(item);
                _logger.LogInformation($"item {idArg} updated");
               
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(UpdateItem)}: {ex.Message}.{ex.InnerException?.Message}");
                return BadRequest($"Could not update. Error {ex.Message}.{ex.InnerException?.Message}");
            }
        }

        [HttpPost()]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<IEmployee>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateItem([FromBody] EmployeeCuDto item)
        {
            try
            {
                _logger.LogInformation($"{nameof(CreateItem)}:");
                
                var model = await _service.CreateEmployeeAsync(item);
                _logger.LogInformation($"item {model.Item.EmployeeId} created");

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateItem)}: {ex.Message}.{ex.InnerException?.Message}");
                return BadRequest($"Could not create. Error {ex.Message}.{ex.InnerException?.Message}");
            }
        }
    }
}

