using Models;
using Models.DTO;

namespace Services;

public interface IAttractionService {

    public Task<ResponsePageDto<IAttractionModel>> ReadAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IAttractionModel>> ReadItemAsync(Guid id, bool flat);
    public Task<ResponseItemDto<IAttractionModel>> DeleteAttractionAsync(Guid id);
    public Task<ResponseItemDto<IAttractionModel>> UpdateAttractionAsync(AttractionCuDto item);
    public Task<ResponseItemDto<IAttractionModel>> CreateAttractionAsync(AttractionCuDto item);



  //  public Task<ResponsePageDto<IBanks>> ReadCreditCardsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
  //  public Task<ResponseItemDto<IBanks>> ReadCreditCardAsync(Guid id, bool flat);
   // public Task<ResponseItemDto<IBanks>> DeleteCreditCardAsync(Guid id);
    public Task<ResponseItemDto<IBanks>> CreateCreditCardAsync(BankCuDto item);
//

}