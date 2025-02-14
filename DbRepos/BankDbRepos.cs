using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;
using Configuration;

namespace DbRepos;

public class BankDbRepos
{
    private readonly ILogger<BankDbRepos> _logger;
    private readonly MainDbContext _dbContext;
    private Encryptions _encryptions;

    #region contructors
    public BankDbRepos(ILogger<BankDbRepos> logger, Encryptions encryptions, MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<IBank>> ReadItemAsync(Guid id, bool flat)
    {
        IQueryable<BankDbM> query;
        if (!flat)
        {
            query = _dbContext.Banks.AsNoTracking()
                .Include(i => i.AttractionDbM)
                .Where(i => i.BankId == id);
        }
        else
        {
            query = _dbContext.Banks.AsNoTracking()
                .Where(i => i.BankId == id);
        }

        var resp = await query.FirstOrDefaultAsync<Bank>();
        return new ResponseItemDto<IBank>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = resp
        };
    }

    public async Task<ResponsePageDto<IBank>> ReadItemsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";
        IQueryable<BankDbM> query;
        if (flat)
        {
            query = _dbContext.Banks.AsNoTracking();
            
        }
        else
        {
            query = _dbContext.Banks.AsNoTracking()
                .Include(i => i.AttractionDbM);
                
        }

        var ret = new ResponsePageDto<IBank>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            DbItemsCount = await query

            //Adding filter functionality
            .Where(i => (i.Seeded == seeded) && 
                        (i.BankComment.ToLower().Contains(filter) ||
                        i.strBank.ToLower().Contains(filter)))
            .CountAsync(),

            PageItems = await query

            //Adding filter functionality
            .Where(i => (i.Seeded == seeded) && 
                        (i.BankComment.ToLower().Contains(filter) ||
                        i.strBank.ToLower().Contains(filter)))

            //Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<IBank>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };
        return ret;
    }

    public async Task<ResponseItemDto<IBank>> DeleteBankAsync(Guid id)
    {
        var query1 = _dbContext.Banks
            .Where(i => i.BankId == id);

        var item = await query1.FirstOrDefaultAsync<BankDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {id} is not existing");

        //delete in the database model
        _dbContext.Banks.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<IBank>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = item
        };
    }


    public async Task<ResponseItemDto<IBank>> CreateBankAsync(BankCuDto itemDto)
    {
        if (itemDto.BankId != null)
            throw new ArgumentException($"{nameof(itemDto.BankId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties
        var item = new BankDbM(itemDto);

        item.EnryptAndObfuscate(_encryptions.AesEncryptToBase64);

        //Update navigation properties
        await navProp_ItemCUdto_to_ItemDbM(itemDto, item);

        //write to database model
        _dbContext.Banks.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadItemAsync(item.BankId, false);    
    }

    //CRUD support
    private async Task navProp_ItemCUdto_to_ItemDbM(BankCuDto itemDtoSrc, BankDbM itemDst)
    {
        //update Employee nav props
        var employee = await _dbContext.Attractions.FirstOrDefaultAsync(
            a => (a.AttractionId == itemDtoSrc.AttractionId));

        if (employee == null)
            throw new ArgumentException($"Item id {itemDtoSrc.AttractionId} not existing");

        itemDst.AttractionDbM = employee;
    }

    //Special Non-CRUD repo
    public async Task<ResponsePageDto<IAttraction>> ReadAttractionsWithCCAsync(bool hasBank, int pageNumber, int pageSize)
    {
        var query = _dbContext.Attractions.AsNoTracking()
            .Include(i => i.BankDbM);

        var ret = new ResponsePageDto<IAttraction>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            DbItemsCount = await query

                //Adding filter functionality
                .Where(i => i.BankDbM == null).CountAsync(),

            PageItems = await query

                //Adding filter functionality
                .Where(i =>(hasBank) ?i.BankDbM != null : i.BankDbM == null)

                //Adding paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<IAttraction>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };
        return ret;
    }

    public async Task<ResponseItemDto<IBank>> ReadDecryptedCCAsync(Guid id)
    {
        IQueryable<BankDbM> query = _dbContext.Banks.AsNoTracking()
                .Include(i => i.AttractionDbM)
                .Where(i => i.BankId == id);

        var resp = await query.FirstOrDefaultAsync<Bank>();
        var cc = resp.Decrypt(_encryptions.AesDecryptFromBase64<Bank>);

        //Nav props are not set in the decrypted object, set them
        cc.Attraction = resp.Attraction;

        return new ResponseItemDto<IBank>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = cc
        };
    }
}
