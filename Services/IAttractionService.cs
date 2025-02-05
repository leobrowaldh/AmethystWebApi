using Models;
using Models.DTO;

namespace Services;

public interface IAttractionService {

    public Task<ResponsePageDto<IAttractionModel>> ReadAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IAttractionModel>> ReadItemAsync(Guid id, bool flat);
    public Task<ResponseItemDto<IAttractionModel>> DeleteAttractionAsync(Guid id);
    public Task<ResponseItemDto<IAttractionModel>> UpdateAttractionAsync(AttractionCuDto item);
    public Task<ResponseItemDto<IAttractionModel>> CreateAttractionAsync(AttractionCuDto item);



   public Task<ResponsePageDto<IBank>> ReadBankAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
   public Task<ResponseItemDto<IBank>> ReadBanksAsync(Guid id, bool flat);
   public Task<ResponseItemDto<IBank>> DeleteBankAsync(Guid id);
   public Task<ResponseItemDto<IBank>> CreateBankAsync(BankCuDto item);
     public Task<ResponsePageDto<IAttractionModel>> ReadAttractionsWithCCAsync(bool hasbank, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IBank>> ReadDecryptedCCAsync(Guid id);


}