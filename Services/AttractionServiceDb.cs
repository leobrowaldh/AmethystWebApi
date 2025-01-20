using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;


public class AttractionServiceDb : IAttractionService {

    private readonly AttractionDbRepos _attractionRepo;
    private readonly ILogger<AttractionServiceDb> _logger;    
    
    public AttractionServiceDb(AttractionDbRepos attractionRepo, ILogger<AttractionServiceDb> logger)
    {
        _attractionRepo = attractionRepo;
        _logger = logger;
    }

    public Task<List<IAttractionModel>> ReadAsync() => _attractionRepo.ReadAsync();
    public Task<IAttractionModel> ReadItemAsync(Guid id) => _attractionRepo.ReadItemAsync(id);
}