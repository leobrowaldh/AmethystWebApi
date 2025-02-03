using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public class AddressDbRepos
{
    private readonly ILogger<AddressDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    #region contructors
    public AddressDbRepos(ILogger<AddressDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<IAddress>> ReadItemAsync(Guid id, bool flat)
    {
        IQueryable<AddressDbM> query;
        if (!flat)
        {
            query = _dbContext.Addresses.AsNoTracking()
                .Include(i => i.AttractionModelDbM) // Double check if it should connect this way.
                .Where(i => i.AddressId == id);
        }
        else
        {
            query = _dbContext.Address.AsNoTracking()
                .Where(i => i.AddressId == id);
        }

        var resp = await query.FirstOrDefaultAsync<IAddress>();
        return new ResponseItemDto<IAddress>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = resp
        };
    }

    public async Task<ResponsePageDto<IAddress>> ReadItemsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";
        IQueryable<AddressDbM> query;
        if (flat)
        {
            query = _dbContext.Address.AsNoTracking();
        }
        else
        {
            query = _dbContext.Address.AsNoTracking()
                .Include(i => i.AttractionModelDbM);
        }

        var ret = new ResponsePageDto<IAddress>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            DbItemsCount = await query

            //Adding filter functionality
            .Where(i => (i.Seeded == seeded) && 
                        (i.strCity.ToLower().Contains(filter) ||
                         i.strCountry.ToLower().Contains(filter) ||
                         i.StreetName.ToLower().Contains(filter) ||
                         i.ZipCode.ToString().Contains(filter))).CountAsync(),

            PageItems = await query

            //Adding filter functionality
            .Where(i => (i.Seeded == seeded) && 
                        (i.strCity.ToLower().Contains(filter) ||
                         i.strCountry.ToLower().Contains(filter) ||
                         i.StreetName.ToLower().Contains(filter) ||
                         i.ZipCode.ToString().Contains(filter)))

            //Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<IAddress>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };
        return ret;
    }

    public async Task<ResponseItemDto<IAddress>> DeleteItemAsync(Guid id)
    {
        var query1 = _dbContext.Addresses
            .Where(i => i.AddressId == id);

        var item = await query1.FirstOrDefaultAsync<AddressDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {id} is not existing");

        //delete in the database model
        _dbContext.Addresses.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<IAddress>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = item
        };
    }

    public async Task<ResponseItemDto<IAddress>> UpdateItemAsync(AddressCuDto itemDto)
    {
        var query1 = _dbContext.Addresses
            .Where(i => i.AddressId == itemDto.AddressId);
        var item = await query1
                .Include(i => i.AttractionModelDbM)
                .FirstOrDefaultAsync<AddressDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {itemDto.AddressId} is not existing");

        //transfer any changes from DTO to database objects
        //Update individual properties 
        item.UpdateFromDTO(itemDto);

        //Update navigation properties
        await navProp_ItemCUdto_to_ItemDbM(itemDto, item);

        //write to database model
        _dbContext.Addresses.Update(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadItemAsync(item.AddressId, false);    
    }

    public async Task<ResponseItemDto<IAddress>> CreateItemAsync(AddressCuDto itemDto)
    {
        if (itemDto.AddressId != null)
            throw new ArgumentException($"{nameof(itemDto.AddressId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties
        var item = new AddressDbM(itemDto);

        //Update navigation properties
        await navProp_ItemCUdto_to_ItemDbM(itemDto, item);

        //write to database model
        _dbContext.Addresses.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadItemAsync(item.AddressId, false);    
    }

    private async Task navProp_ItemCUdto_to_ItemDbM(AddressCuDto itemDtoSrc, AddressDbM itemDst)
    {
        //update zoo nav props
        var zoo = await _dbContext.Attractions.FirstOrDefaultAsync(
            a => (a.AttractionId == itemDtoSrc.AttractionId));

        if (zoo == null)
            throw new ArgumentException($"Item id {itemDtoSrc.AttractionId} not existing");

        itemDst.AttractionModelDbM = zoo;
    }
}
