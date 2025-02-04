using Models;
using Models.DTO;

namespace Services;

public interface IAttractionService {

    public Task<ResponsePageDto<IAttractionModel>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IAttractionModel>> ReadAttractionAsync(Guid id, bool flat);
    public Task<ResponseItemDto<IAttractionModel>> DeleteAttractionAsync(Guid id);
    public Task<ResponseItemDto<IAttractionModel>> UpdateAttractionAsync(AttractionCuDto item);
    public Task<ResponseItemDto<IAttractionModel>> CreateAttractionAsync(AttractionCuDto item);
    public Task<ResponsePageDto<IComment>> ReadCommentsAsync(bool seeded,bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IComment>> ReadCommentAsync(Guid id,bool flat);
    public Task<ResponseItemDto<IComment>> DeleteCommentAsync(Guid id);
    public Task<ResponseItemDto<IComment>> UpdateCommentAsync(CommentCuDto item);
    public Task<ResponseItemDto<IComment>> CreateCommentAsync(CommentCuDto item);
    public Task<ResponsePageDto<IComment>> ReadAddressesAsync(bool seeded,bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IComment>> ReadAddressAsync(Guid id,bool flat);
    public Task<ResponseItemDto<IComment>> DeleteAddressAsync(Guid id);
    public Task<ResponseItemDto<IComment>> UpdateAddressAsync(CommentCuDto item);
    public Task<ResponseItemDto<IComment>> CreateAddressAsync(CommentCuDto item);


}