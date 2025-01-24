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

    public async Task<ResponseItemDto<IAttractionModel>> ReadItemAsync(Guid id)
    {
        IQueryable<AttractionModelDbM> query;

        query = _dbContext.Attractions.AsNoTracking()
            .Where(a => a.Id == id);

        var resp = await query.FirstOrDefaultAsync<IAttractionModel>();
        return new ResponseItemDto<IAttractionModel>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = resp
        };
    }

    public async Task<ResponsePageDto<IAttractionModel>> ReadItemsAsync(bool seeded, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";
        filter = filter.ToLower();

        // Try to parse the filter into an enum value
        AttractionCategory? categoryFilter = Enum.GetValues(typeof(AttractionCategory))
            .Cast<AttractionCategory>()
            .FirstOrDefault(e => e.ToString().ToLower() == filter);

        IQueryable<AttractionModelDbM> query;

        query = _dbContext.Attractions.AsNoTracking();
        

        return new ResponsePageDto<IAttractionModel>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            DbItemsCount = await query

            //Adding filter functionality
            .Where(a => (a.Seeded == seeded) &&
                        (categoryFilter != null && a.Category == categoryFilter ||
                         a.Name.ToLower().Contains(filter) ))
            .CountAsync(),

            PageItems = await query

            //Adding filter functionality
            .Where(a => (a.Seeded == seeded) &&
                        (a.Name.ToLower().Contains(filter) ||
                         categoryFilter != null && a.Category == categoryFilter))

            //Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<IAttractionModel>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };
    }

}