using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;


public class AttractionServiceDb : IAttractionService
{

    private readonly AttractionDbRepos _attractionRepo;
    private readonly ILogger<AttractionServiceDb> _logger;    
    
    public AttractionServiceDb(AttractionDbRepos attractionRepo, ILogger<AttractionServiceDb> logger)
    {
        _attractionRepo = attractionRepo;
        _logger = logger;
    }

    public Task<ResponsePageDto<IAttractionModel>> ReadAsync(bool seeded, string filter, int pageNumber, int pageSize) => _attractionRepo.ReadItemsAsync(seeded, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IAttractionModel>> ReadItemAsync(Guid id) => _attractionRepo.ReadItemAsync(id);
    public Task<ResponseItemDto<IAttractionModel>> DeleteAttractionAsync(Guid id) => _attractionRepo.DeleteAttractionAsync(id);
    
     public Task<ResponseItemDto<IAttractionModel>> UpdateAttractionAsync(AttractionCuDto item)=> _attractionRepo.UpdateAttractionAsync(item);


}