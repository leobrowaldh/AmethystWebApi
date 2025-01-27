using Models;
using Models.DTO;

namespace Services;

public interface ICommentService {

    public Task<ResponsePageDto<IComment>> ReadAsync(bool seeded, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IComment>> ReadCommentAsync(Guid id);
    //public Task<ResponseItemDto<IComment>> DeleteCommentAsync(Guid id);
   // public Task<ResponseItemDto<IComment>> UpdateCommentAsync(CommentCuDto item);
   // public Task<ResponseItemDto<IComment>> CreateCommentAsync(CommentCuDto item);


}