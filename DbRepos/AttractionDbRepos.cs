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

     public async Task<ResponseItemDto<IAttractionModel>> DeleteAttractionAsync(Guid id)
    {
        //Find the instance with matching id
        var query1 = _dbContext.Attractions
            .Where(i => i.Id == id);
        var item = await query1.FirstOrDefaultAsync<AttractionModelDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {id} is not existing");

        //delete in the database model
        _dbContext.Attractions.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<IAttractionModel>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = item
        };
    }

      public async Task<ResponseItemDto<IAttractionModel>> UpdateAttractionAsync(AttractionCuDto itemDto)
    {
        //Find the instance with matching id and read the navigation properties.
        var query1 = _dbContext.Attractions
            .Where(i => i.Id == itemDto.Id);
        var item = await query1
            .FirstOrDefaultAsync<AttractionModelDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {itemDto.Id} is not existing");

        //transfer any changes from DTO to database objects
        //Update individual properties
        item.UpdateFromDTO(itemDto);

        //write to database model
        _dbContext.Attractions.Update(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadItemAsync(item.Id);    
    }


}