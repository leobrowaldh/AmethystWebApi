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
        _CommentRepo = CommentRepo;
        _logger = logger;
    }

    public Task<ResponsePageDto<IComment>> ReadAsync(bool seeded, string filter, int pageNumber, int pageSize) => _CommentRepo.ReadItemsAsync(seeded, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IComment>> ReadItemAsync(Guid id) => _CommentRepo.ReadItemAsync(id);
    public Task<ResponseItemDto<IComment>> DeleteAttractionAsync(Guid id) => _CommentRepo.DeleteAttractionAsync(id);
    public Task<ResponseItemDto<IComment>> UpdateAttractionAsync(Comment item)=> _CommentRepo.UpdateAttractionAsync(item);
    public Task<ResponseItemDto<IComment>> CreateAttractionAsync(Comment item) => _CommentRepo.CreateItemAsync(item);

    public Task<ResponseItemDto<IComment>> ReadCommentAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}