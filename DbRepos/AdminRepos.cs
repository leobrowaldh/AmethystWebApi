﻿using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;

using Models.DTO;
using DbModels;
using DbContext;
using Models;
using Seido.Utilities.SeedGenerator;

namespace DbRepos;

public class AdminDbRepos
{
    private readonly ILogger<AdminDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    #region contructors
    public AdminDbRepos(ILogger<AdminDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync()
    {
        var info = new GstUsrInfoAllDto();
        info.Db = await _dbContext.InfoDbView.FirstAsync();

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

        var attractions = rnd.ItemsToList<AttractionModelDbM>(nrOfItems);
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
}
