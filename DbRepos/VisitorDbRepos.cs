using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public class VisitorDbRepos
{
    private readonly ILogger<VisitorDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    #region contructors
    public VisitorDbRepos(ILogger<VisitorDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<IVisitor>> ReadItemAsync(Guid id, bool flat)
    {
        IQueryable<VisitorDbM> query;
        if (!flat)
        {
            query = _dbContext.Visitors.AsNoTracking()
                .Include(i => i.AttractionModelsDbM)
                .Where(i => i.VisitorId == id);
        }
        else
        {
            query = _dbContext.Visitors.AsNoTracking()
                .Where(i => i.VisitorId == id);
        }

        var resp = await query.FirstOrDefaultAsync<IVisitor>();
        return new ResponseItemDto<IVisitor>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = resp
        };
    }

    public async Task<ResponsePageDto<IVisitor>> ReadItemsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";
        IQueryable<VisitorDbM> query;
        if (flat)
        {
            query = _dbContext.Visitors.AsNoTracking();
        }
        else
        {
            query = _dbContext.Visitors.AsNoTracking()
                .Include(i => i.AttractionModelsDbM)
        }

        var ret = new ResponsePageDto<IVisitor>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            DbItemsCount = await query

            //Adding filter functionality
            .Where(i => (i.Seeded == seeded) && 
                        (i.VisitorFirstName.ToLower().Contains(filter) ||
                         i.VisitorLastName.ToLower().Contains(filter) ||
                         i.strRole.ToLower().Contains(filter))).CountAsync(),

            PageItems = await query

            //Adding filter functionality
            .Where(i => (i.Seeded == seeded) && 
                        (i.VisitorFirstName.ToLower().Contains(filter) ||
                         i.VisitorLastName.ToLower().Contains(filter) ||
                         i.strRole.ToLower().Contains(filter)))

            //Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<IVisitor>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };
        return ret;
    }

    public async Task<ResponseItemDto<IVisitor>> DeleteItemAsync(Guid id)
    {
        var query1 = _dbContext.Visitors
            .Where(i => i.VisitorId == id);

        var item = await query1.FirstOrDefaultAsync<VisitorDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {id} is not existing");

        //delete in the database model
        _dbContext.Visitors.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<IVisitor>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = item
        };
    }

    public async Task<ResponseItemDto<IVisitor>> UpdateItemAsync(VisitorCuDto itemDto)
    {
        var query1 = _dbContext.Visitors
            .Where(i => i.VisitorId == itemDto.EmployeeId);
        var item = await query1
                .Include(i => i.AttractionModelsDbM)
                .FirstOrDefaultAsync<VisitorDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {itemDto.EmployeeId} is not existing");

        //transfer any changes from DTO to database objects
        //Update individual properties 
        item.UpdateFromDTO(itemDto);

        //Update navigation properties
        await navProp_ItemCUdto_to_ItemDbM(itemDto, item);

        //write to database model
        _dbContext.Visitor.Update(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadItemAsync(item.VisitorId, false);    
    }

    public async Task<ResponseItemDto<IVisitor>> CreateItemAsync(VisitorCuDto itemDto)
    {
        if (itemDto.VisitorId != null)
            throw new ArgumentException($"{nameof(itemDto.VisitorId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties
        var item = new VisitorDbM(itemDto);

        //Update navigation properties
        await navProp_ItemCUdto_to_ItemDbM(itemDto, item);

        //write to database model
        _dbContext.Visitor.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadItemAsync(item.VisitorId, false);    
    }

    private async Task navProp_ItemCUdto_to_ItemDbM(VisitorCuDto itemDtoSrc, VisitorDbM itemDst)
    {
        //update ZooDbM from itemDto.ZooId
        List<AttractionModelDbM> attractions = null;
        if (itemDtoSrc.AttractionIds != null)
        {
            attractions = new List<AttractionModelDbM>();
            foreach (var id in itemDtoSrc.ZooIds)
            {
                var z = await _dbContext.Attractions.FirstOrDefaultAsync(i => i.AttractionId == id);
                if (z == null)
                    throw new ArgumentException($"Item id {id} not existing");

                attractions.Add(z);
            }
        }
        itemDst.AttractionModelsDbM = attractions;
    }
}
