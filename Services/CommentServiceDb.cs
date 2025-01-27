using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;


public class CommentServiceDb : ICommentService {

    private readonly  CommentDbRepos _CommentRepo;
    private readonly ILogger<CommentServiceDb> _logger;    
    
    public CommentServiceDb(CommentDbRepos commentRepo, ILogger<CommentServiceDb> logger)
    {
        _CommentRepo = commentRepo;
        _logger = logger;
    }

    public Task<ResponsePageDto<IComment>> ReadAsync(bool seeded,bool flat, string filter, int pageNumber, int pageSize) => _CommentRepo.ReadItemsAsync(seeded,flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IComment>> ReadCommentAsync(Guid id,bool flat) => _CommentRepo.ReadItemAsync(id,flat);
   // public Task<ResponseItemDto<IComment>> DeleteAttractionAsync(Guid id) => _CommentRepo.DeleteAttractionAsync(id);
   // public Task<ResponseItemDto<IComment>> UpdateAttractionAsync(Comment item)=> _CommentRepo.UpdateAttractionAsync(item);
   // public Task<ResponseItemDto<IComment>> CreateAttractionAsync(Comment item) => _CommentRepo.CreateItemAsync(item);

   
}