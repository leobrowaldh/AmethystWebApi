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
    private readonly AddressDbRepos _addressRepo;
    public AttractionServiceDb(AttractionDbRepos attractionRepo, CommentDbRepos commentRepo, AddressDbRepos addressRepo, ILogger<AttractionServiceDb> logger)
    {
        _attractionRepo = attractionRepo;
        _commentRepo = commentRepo;
        _addressRepo = addressRepo;
        _logger = logger;
    }

    public Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _attractionRepo.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat) => _attractionRepo.ReadItemAsync(id, flat);
    public Task<ResponseItemDto<IAttraction>> DeleteAttractionAsync(Guid id) => _attractionRepo.DeleteAttractionAsync(id);
    public Task<ResponseItemDto<IAttraction>> UpdateAttractionAsync(AttractionCuDto item)=> _attractionRepo.UpdateAttractionAsync(item);
    public Task<ResponseItemDto<IAttraction>> CreateAttractionAsync(AttractionCuDto item) => _attractionRepo.CreateItemAsync(item);

    public Task<ResponsePageDto<IComment>> ReadCommentsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _commentRepo.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);

    public Task<ResponseItemDto<IComment>> ReadCommentAsync(Guid id, bool flat) => _commentRepo.ReadItemAsync(id, flat);

    public Task<ResponseItemDto<IComment>> DeleteCommentAsync(Guid id) => _commentRepo.DeleteItemAsync(id);

    public Task<ResponseItemDto<IComment>> UpdateCommentAsync(CommentCuDto item) => _commentRepo.UpdateItemAsync(item);

    public Task<ResponseItemDto<IComment>> CreateCommentAsync(CommentCuDto item) => _commentRepo.CreateItemAsync(item);
    public Task<ResponsePageDto<IAddress>> ReadAddressesAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize) => _addressRepo.ReadItemsAsync(seeded, flat, filter, pageNumber, pageSize);

    public Task<ResponseItemDto<IAddress>> ReadAddressAsync(Guid id, bool flat) => _addressRepo.ReadItemAsync(id, flat);

    public Task<ResponseItemDto<IAddress>> DeleteAddressAsync(Guid id) => _addressRepo.DeleteItemAsync(id);

    public Task<ResponseItemDto<IAddress>> UpdateAddressAsync(AddressCuDto item) => _addressRepo.UpdateItemAsync(item);

    public Task<ResponseItemDto<IAddress>> CreateAddressAsync(AddressCuDto item) => _addressRepo.CreateItemAsync(item);
}