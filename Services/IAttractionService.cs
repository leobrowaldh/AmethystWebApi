using Models;
using Models.DTO;

namespace Services;

public interface IAttractionService {

    public Task<ResponsePageDto<IAttractionModel>> ReadAsync(bool seeded, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IAttractionModel>> ReadItemAsync(Guid id);
}