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

    public async Task<ResponseItemDto<IAttractionModel>> ReadItemAsync(Guid id, bool flat)
    {
        IQueryable<AttractionModelDbM> query;

        if (!flat)
        {
            query = _dbContext.Attractions.AsNoTracking()
                .Include(a => a.Comments)
                .Where(a => a.AttractionId == id);
        }
        else
        {
            query = _dbContext.Attractions.AsNoTracking()
                .Where(a => a.AttractionId == id);
        }  

        

        var resp = await query.FirstOrDefaultAsync<IAttractionModel>();
        return new ResponseItemDto<IAttractionModel>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = resp
        };
    }

    public async Task<ResponsePageDto<IAttractionModel>> ReadItemsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";
        filter = filter.ToLower();

        IQueryable<AttractionModelDbM> query;
        
        if (flat)
        {
            query = _dbContext.Attractions.AsNoTracking();
        }
        else
        {
            query = _dbContext.Attractions.AsNoTracking()
                .Include(a => a.Comments);
        }

        return new ResponsePageDto<IAttractionModel>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            DbItemsCount = await query

            //Adding filter functionality
            .Where(a => (a.Seeded == seeded) &&
                        (a.strCategory.ToLower().Contains(filter) ||
                         a.Name.ToLower().Contains(filter) ))
            .CountAsync(),

            PageItems = await query

            //Adding filter functionality
            .Where(a => (a.Seeded == seeded) &&
                        (a.Name.ToLower().Contains(filter) ||
                         a.strCategory.ToLower().Contains(filter)))

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
            .Where(i => i.AttractionId == id);
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
            .Where(i => i.AttractionId == itemDto.AttractionId);
        var item = await query1
            .FirstOrDefaultAsync<AttractionModelDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {itemDto.AttractionId} is not existing");

        //transfer any changes from DTO to database objects
        //Update individual properties
        item.UpdateFromDTO(itemDto);

        //write to database model
        _dbContext.Attractions.Update(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadItemAsync(item.AttractionId, false);    
    }

    public async Task<ResponseItemDto<IAttractionModel>> CreateItemAsync(AttractionCuDto itemDto)
    {
        if (itemDto.AttractionId != null)
            throw new ArgumentException($"{nameof(itemDto.AttractionId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties Zoo
        var item = new AttractionModelDbM(itemDto);

        //write to database model
        _dbContext.Attractions.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadItemAsync(item.AttractionId, false);
    }


}