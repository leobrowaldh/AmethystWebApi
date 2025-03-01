﻿using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

using Models.DTO;
using DbModels;
using DbContext;
using Models;
using Seido.Utilities.SeedGenerator;
using Configuration;
using System.Security;

namespace DbRepos;

public class AdminDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    #region contructors
    public AdminDbRepos(ILogger<AdminDbRepos> logger, Encryptions encryptions, MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync()
    {
      
        var info = new GstUsrInfoAllDto();
        info.Db = await _dbContext.InfoDbView.FirstAsync();
        info.Attractions= await _dbContext.InfoAttractionsView.ToListAsync();
        info.Comments= await _dbContext.InfoCommentsView.ToListAsync();
        info.Addresses = await _dbContext.InfoAddressesView.ToListAsync();


        return new ResponseItemDto<GstUsrInfoAllDto>()
        {
            DbConnectionKeyUsed = _dbContext.dbConnection,
            Item = info
        };
    }

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrOfItems)
    {
        //First of all make sure the database is cleared from all seeded data
        await RemoveSeedAsync(true);

        var rnd = new csSeedGenerator();

        var attractions = rnd.ItemsToList<AttractionDbM>(nrOfItems);

        // //Assign Banks to Attractions with 50% probability
          foreach (var a in attractions)
        {
            a.BankDbM = new BankDbM().Seed(rnd);
            a.BankDbM?.EnryptAndObfuscate(_encryptions.AesEncryptToBase64);
#if DEBUG
        var temp = a.BankDbM?.Decrypt(_encryptions.AesDecryptFromBase64<Bank>);
           if (temp?.BankId != a.BankDbM?.BankId) throw new SecurityException("CreditCard encryption error");
#endif
        }

        foreach (var attraction in attractions)
        {
            attraction.AddressDbM = new AddressDbM().Seed(rnd);
            attraction.CommentsDbM = rnd.ItemsToList<CommentDbM>(rnd.Next(0,21));
        }
        _dbContext.Attractions.AddRange(attractions);
        await _dbContext.SaveChangesAsync();
        return await InfoAsync();        
    }
    
    public async Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded)
    {
        var parameters = new List<SqlParameter>();

        var retValue = new SqlParameter("retval", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var seededArg = new SqlParameter("seeded", seeded);

        parameters.Add(retValue);
        parameters.Add(seededArg);

        //there is no FromSqlRawAsync to I make one here
        var _query = await Task.Run(() =>
            _dbContext.InfoDbView.FromSqlRaw($"EXEC @retval = supusr.spDeleteAll @seeded",
                parameters.ToArray()).AsEnumerable());

        //Execute the query and get the sp result set.
        //Although, I am not using this result set, but it shows how to get it
        GstUsrInfoDbDto result_set = _query.FirstOrDefault();

        //Check the return code
        int retCode = (int)retValue.Value;
        if (retCode != 0) throw new Exception("supusr.spDeleteAll return code error");

        return await InfoAsync();
    }

    public async Task<UserDto> SeedUsersAsync(int nrOfUsers, int nrOfSuperUsers, int nrOfSysAdmin)
    {
        _logger.LogInformation($"Seeding {nrOfUsers} users and {nrOfSuperUsers} superusers");
        
        //First delete all existing users
        foreach (var u in _dbContext.Users)
            _dbContext.Users.Remove(u);

        //add users
        for (int i = 1; i <= nrOfUsers; i++)
        {
            _dbContext.Users.Add(new UserDbM
            {
                UserId = Guid.NewGuid(),
                UserName = $"user{i}",
                Email = $"user{i}@gmail.com",
                Password = _encryptions.EncryptPasswordToBase64($"user{i}"),
                Role = "usr"
            });
        }

        //add super user
        for (int i = 1; i <= nrOfSuperUsers; i++)
        {
            _dbContext.Users.Add(new UserDbM
            {
                UserId = Guid.NewGuid(),
                UserName = $"superuser{i}",
                Email = $"superuser{i}@gmail.com",
                Password = _encryptions.EncryptPasswordToBase64($"superuser{i}"),
                Role = "supusr"
            });
        }

        //add system adminitrators
        for (int i = 1; i <= nrOfSysAdmin; i++)
        {
            _dbContext.Users.Add(new UserDbM
            {
                UserId = Guid.NewGuid(),
                UserName = $"sysadmin{i}",
                Email = $"sysadmin{i}@gmail.com",
                Password = _encryptions.EncryptPasswordToBase64($"sysadmin{i}"),
                Role = "sysadmin"
            });
        }
        await _dbContext.SaveChangesAsync();

        var _info = new UserDto
        {
            NrUsers = await _dbContext.Users.CountAsync(i => i.Role == "usr"),
            NrSuperUsers = await _dbContext.Users.CountAsync(i => i.Role == "supusr"),
            NrSystemAdmin = await _dbContext.Users.CountAsync(i => i.Role == "sysadmin")
        };

        return _info;
    }
}
