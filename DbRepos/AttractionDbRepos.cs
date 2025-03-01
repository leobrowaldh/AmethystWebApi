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

    public async Task<ResponseItemDto<IAttraction>> ReadItemAsync(Guid id, bool flat)
    {
        IQueryable<AttractionDbM> query;

        if (!flat)
        {
            query = _dbContext.Attractions.AsNoTracking()
                .Include(a => a.CommentsDbM)
                .Include(a => a.AddressDbM)
                .Where(a => a.AttractionId == id);
        }
        else
        {
            query = _dbContext.Attractions.AsNoTracking()
                .Where(a => a.AttractionId == id);
        }  

        

        var resp = await query.FirstOrDefaultAsync<IAttraction>();
        return new ResponseItemDto<IAttraction>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = resp
        };
    }

    public async Task<ResponsePageDto<IAttraction>> ReadItemsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";
        filter = filter.ToLower();

        IQueryable<AttractionDbM> query;
        
        if (flat && filter == "")
        {
            query = _dbContext.Attractions.AsNoTracking();
        }
        else if (flat)
        {
            query = _dbContext.Attractions.AsNoTracking()
                .Include(a => a.AddressDbM); //Address is needed to filter by city
        }
        else
        {
            query = _dbContext.Attractions.AsNoTracking()
                .Include(a => a.CommentsDbM)
                .Include(a => a.AddressDbM);
        }

        return new ResponsePageDto<IAttraction>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            DbItemsCount = await query

            //Adding filter functionality
            .Where(a => (a.Seeded == seeded) &&
                        (a.strCategory.ToLower().Contains(filter) ||
                         a.Name.ToLower().Contains(filter) ||
                         a.Description.ToLower().Contains(filter) ||
                         a.AddressDbM.strCity.ToLower().Contains(filter) ||
                         a.AddressDbM.strCountry.ToLower().Contains(filter)))
            .CountAsync(),

            PageItems = await query

            //Adding filter functionality
            .Where(a => (a.Seeded == seeded) &&
                        (a.strCategory.ToLower().Contains(filter) ||
                         a.Name.ToLower().Contains(filter) ||
                         a.Description.ToLower().Contains(filter) ||
                         a.AddressDbM.strCity.ToLower().Contains(filter) ||
                         a.AddressDbM.strCountry.ToLower().Contains(filter)))

            //Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<IAttraction>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };


        
    }

    public async Task<ResponseItemDto<IAttraction>> DeleteAttractionAsync(Guid id)
    {
        //Find the instance with matching id
        var query1 = _dbContext.Attractions
            .Where(i => i.AttractionId == id);
        var item = await query1.FirstOrDefaultAsync<AttractionDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {id} is not existing");

        //delete in the database model
        _dbContext.Attractions.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<IAttraction>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = item
        };
    }

    public async Task<ResponseItemDto<IAttraction>> UpdateAttractionAsync(AttractionCuDto itemDto)
    {
        //Find the instance with matching id and read the navigation properties.
        var query1 = _dbContext.Attractions
            .Where(i => i.AttractionId == itemDto.AttractionId);
        var item = await query1
            .FirstOrDefaultAsync<AttractionDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {itemDto.AttractionId} is not existing");

        //transfer any changes from DTO to database objects
        //Update individual properties
        item.UpdateFromDTO(itemDto);

        //Update navigation properties
        await navProp_Itemdto_to_ItemDbM(itemDto, item);
        
        //write to database model
        _dbContext.Attractions.Update(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadItemAsync(item.AttractionId, false);    
    }

    public async Task<ResponseItemDto<IAttraction>> CreateItemAsync(AttractionCuDto itemDto)
    {
        if (itemDto.AttractionId != null)
            throw new ArgumentException($"{nameof(itemDto.AttractionId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties Zoo
        var item = new AttractionDbM(itemDto);

        //Update navigation properties
        await navProp_Itemdto_to_ItemDbM(itemDto, item);

        //write to database model
        _dbContext.Attractions.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadItemAsync(item.AttractionId, false);
    }

    //from all Guid relationships in _itemDtoSrc finds the corresponding object in the database and assigns it to _itemDst 
    //as navigation properties. Error is thrown if no object is found corresponing to an id.
    private async Task navProp_Itemdto_to_ItemDbM(AttractionCuDto itemDtoSrc, AttractionDbM itemDst)
    {
        //update CommentsDbM from list
        List<CommentDbM> comments = null;
        if (itemDtoSrc.CommentIds != null)
        {
            comments = new List<CommentDbM>();
            foreach (var id in itemDtoSrc.CommentIds)
            {
                var p = await _dbContext.Comments.FirstOrDefaultAsync(i => i.CommentId == id);
                if (p == null)
                    throw new ArgumentException($"Item id {id} not existing");

                
                comments.Add(p);
            }
        }
        itemDst.CommentsDbM = comments;

        //update address nav props
        var address = await _dbContext.Addresses.FirstOrDefaultAsync(
            a => (a.AddressId == itemDtoSrc.AddressId));

        if (address == null)
            throw new ArgumentException($"Item id {itemDtoSrc.AddressId} not existing");

        itemDst.AddressDbM = address;
    }

}