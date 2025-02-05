using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;


public class AttractionServiceDb : IAttractionService
{

    private readonly AttractionDbRepos _attractionRepo;
    private readonly  BankDbRepos _bankRepo;
    private readonly ILogger<AttractionServiceDb> _logger;    
    
    public AttractionServiceDb(AttractionDbRepos attractionRepo, BankDbRepos bankDbRepo, ILogger<AttractionServiceDb> logger)
    {
        _attractionRepo = attractionRepo;
        _bankRepo = bankDbRepo;
        _logger = logger;
    }

    public Task<ResponsePageDto<IAttractionModel>> ReadAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _attractionRepo.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IAttractionModel>> ReadItemAsync(Guid id, bool flat) => _attractionRepo.ReadItemAsync(id, flat);
    public Task<ResponseItemDto<IAttractionModel>> DeleteAttractionAsync(Guid id) => _attractionRepo.DeleteAttractionAsync(id);
    public Task<ResponseItemDto<IAttractionModel>> UpdateAttractionAsync(AttractionCuDto item)=> _attractionRepo.UpdateAttractionAsync(item);
    public Task<ResponseItemDto<IAttractionModel>> CreateAttractionAsync(AttractionCuDto item) => _attractionRepo.CreateItemAsync(item);


    public Task<ResponsePageDto<IBank>> ReadBankAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)=> _bankRepo.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);
   public Task<ResponseItemDto<IBank>> ReadBanksAsync(Guid id, bool flat) => _bankRepo.ReadItemAsync(id, flat);
   public Task<ResponseItemDto<IBank>> DeleteBankAsync(Guid id)=> _bankRepo.DeleteBankAsync(id);
   public Task<ResponseItemDto<IBank>> CreateBankAsync(BankCuDto item)=> _bankRepo.CreateBankAsync(item);
     public Task<ResponsePageDto<IAttractionModel>> ReadAttractionsWithCCAsync(bool hasbank, int pageNumber, int pageSize)=> _bankRepo.ReadAttractionsWithCCAsync( hasbank, pageNumber, pageSize);
    public Task<ResponseItemDto<IBank>> ReadDecryptedCCAsync(Guid id)=> _bankRepo.ReadDecryptedCCAsync(id);
}