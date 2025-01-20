using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

using Models.DTO;
using DbModels;
using DbContext;
using Models;

namespace DbRepos;

public class AttractionDbRepos
{
    private readonly ILogger<AdminDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public AttractionDbRepos(ILogger<AdminDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task<List<IAttractionModel>> ReadAsync()
    {
        //here we specify the list type between <> :
        var at = await _dbContext.AttractionModels.OrderBy(am => am.Name).ToListAsync<IAttractionModel>();
        return at;
    }

    public async Task<IAttractionModel> ReadItemAsync(Guid id)
    {
        //No need for firstordefault, since we catch the errors in the controller.
        var at = await _dbContext.AttractionModels.FirstAsync(a => a.Id == id);
        return at;
    }

}