using DbRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.DTO;

namespace Services;


public class AttractionServiceDb : IAttractionService
{

    private readonly AttractionDbRepos _attractionRepo;
    private readonly CommentDbRepos _commentRepo;
    private readonly ILogger<AttractionServiceDb> _logger;    
    
    public AttractionServiceDb(AttractionDbRepos attractionRepo, CommentDbRepos commentRepo, ILogger<AttractionServiceDb> logger)
    {
        _attractionRepo = attractionRepo;
        _logger = logger;
        _commentRepo = commentRepo;
    }

    public Task<ResponsePageDto<IAttractionModel>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _attractionRepo.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IAttractionModel>> ReadAttractionAsync(Guid id, bool flat) => _attractionRepo.ReadItemAsync(id, flat);
    public Task<ResponseItemDto<IAttractionModel>> DeleteAttractionAsync(Guid id) => _attractionRepo.DeleteAttractionAsync(id);
    public Task<ResponseItemDto<IAttractionModel>> UpdateAttractionAsync(AttractionCuDto item)=> _attractionRepo.UpdateAttractionAsync(item);
    public Task<ResponseItemDto<IAttractionModel>> CreateAttractionAsync(AttractionCuDto item) => _attractionRepo.CreateItemAsync(item);

    public Task<ResponsePageDto<IComment>> ReadCommentsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _commentRepo.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);

    public Task<ResponseItemDto<IComment>> ReadCommentAsync(Guid id, bool flat) => _commentRepo.ReadItemAsync(id, flat);

    public Task<ResponseItemDto<IComment>> DeleteCommentAsync(Guid id) => _commentRepo.DeleteItemAsync(id);

    public Task<ResponseItemDto<IComment>> UpdateCommentAsync(CommentCuDto item) => _commentRepo.UpdateItemAsync(item);

    public Task<ResponseItemDto<IComment>> CreateCommentAsync(CommentCuDto item) => _commentRepo.CreateItemAsync(item);

    public Task<ResponsePageDto<IComment>> ReadAddressesAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IComment>> ReadAddressAsync(Guid id, bool flat)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IComment>> DeleteAddressAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IComment>> UpdateAddressAsync(CommentCuDto item)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemDto<IComment>> CreateAddressAsync(CommentCuDto item)
    {
        throw new NotImplementedException();
    }
}