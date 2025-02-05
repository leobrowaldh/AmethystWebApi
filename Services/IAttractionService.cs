using Models;
using Models.DTO;

namespace Services;

public interface IAttractionService {

    public Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat);
    public Task<ResponseItemDto<IAttraction>> DeleteAttractionAsync(Guid id);
    public Task<ResponseItemDto<IAttraction>> UpdateAttractionAsync(AttractionCuDto item);
    public Task<ResponseItemDto<IAttraction>> CreateAttractionAsync(AttractionCuDto item);
    public Task<ResponsePageDto<IComment>> ReadCommentsAsync(bool seeded,bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IComment>> ReadCommentAsync(Guid id,bool flat);
    public Task<ResponseItemDto<IComment>> DeleteCommentAsync(Guid id);
    public Task<ResponseItemDto<IComment>> UpdateCommentAsync(CommentCuDto item);
    public Task<ResponseItemDto<IComment>> CreateCommentAsync(CommentCuDto item);
    public Task<ResponsePageDto<IAddress>> ReadAddressesAsync(bool seeded,bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IAddress>> ReadAddressAsync(Guid id,bool flat);
    public Task<ResponseItemDto<IAddress>> DeleteAddressAsync(Guid id);
    public Task<ResponseItemDto<IAddress>> UpdateAddressAsync(AddressCuDto item);
    public Task<ResponseItemDto<IAddress>> CreateAddressAsync(AddressCuDto item);



   public Task<ResponsePageDto<IBank>> ReadBankAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
   public Task<ResponseItemDto<IBank>> ReadBanksAsync(Guid id, bool flat);
   public Task<ResponseItemDto<IBank>> DeleteBankAsync(Guid id);
   public Task<ResponseItemDto<IBank>> CreateBankAsync(BankCuDto item);
     public Task<ResponsePageDto<IAttraction>> ReadAttractionsWithCCAsync(bool hasbank, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IBank>> ReadDecryptedCCAsync(Guid id);


}