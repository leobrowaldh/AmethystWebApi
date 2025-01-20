using Models;
using Models.DTO;

namespace Services;

public interface IAttractionService {

    public Task<List<IAttractionModel>> ReadAsync();
    public Task<IAttractionModel> ReadItemAsync(Guid id);
}