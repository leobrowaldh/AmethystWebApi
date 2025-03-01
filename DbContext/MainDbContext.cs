﻿using System.Data;
using Microsoft.EntityFrameworkCore;

using Configuration;
using Models;
using Models.DTO;
using DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Npgsql.Replication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DbContext;

//DbContext namespace is a fundamental EFC layer of the database context and is
//used for all Database connection as well as for EFC CodeFirst migration and database updates 

public class MainDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    IConfiguration _configuration;
    DatabaseConnections _databaseConnections;

    public string dbConnection  => _databaseConnections.GetDbConnection(this.Database.GetConnectionString());

    #region C# model of database tables
    public DbSet<AttractionDbM> Attractions { get; set; }
    public DbSet<CommentDbM> Comments { get; set; }
    public DbSet<AddressDbM> Addresses { get; set; }
    public DbSet<UserDbM> Users { get; set; }   
    public DbSet<BankDbM> Banks { get; set; }
    #endregion

    #region model the Views
    public DbSet<GstUsrInfoDbDto> InfoDbView { get; set; }
    public DbSet<GstUsrInfoAttractionsDto> InfoAttractionsView { get; set; }
    public DbSet<GstUsrInfoCommentsDto> InfoCommentsView { get; set; }
    public DbSet<GstUsrInfoAddressesDto> InfoAddressesView { get; set; }
    #endregion

    #region constructors
    public MainDbContext() { }
    public MainDbContext(DbContextOptions options, IConfiguration configuration, DatabaseConnections databaseConnections) : base(options)
    { 
        _databaseConnections = databaseConnections;
        _configuration = configuration;
    }
    #endregion

    //Here we can modify the migration building
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        #region model the Views
        modelBuilder.Entity<GstUsrInfoDbDto>().ToView("vwInfoDb", "gstusr").HasNoKey();
        modelBuilder.Entity<GstUsrInfoAttractionsDto>().ToView("vwInfoAttractions", "gstusr").HasNoKey();
        modelBuilder.Entity<GstUsrInfoCommentsDto>().ToView("vwInfoComments", "gstusr").HasNoKey();
        modelBuilder.Entity<GstUsrInfoAddressesDto>().ToView("vwInfoAddresses", "gstusr").HasNoKey();
        #endregion

        #region override modelbuilder
        //Tokens can be very long
        modelBuilder.Entity<BankDbM>()
            .Property(a => a.EncryptedToken).HasColumnType("nvarchar(max)");
        #endregion
        
        base.OnModelCreating(modelBuilder);
    }

    #region DbContext for some popular databases
    public class SqlServerDbContext : MainDbContext
    {
        public SqlServerDbContext() { }
        public SqlServerDbContext(DbContextOptions options, IConfiguration configuration, DatabaseConnections databaseConnections) 
            : base(options, configuration, databaseConnections) { }


        //Used only for CodeFirst Database Migration and database update commands
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = SetupDatabaseConnection(optionsBuilder, 
                    (options, connectionString) => options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure()));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HaveColumnType("money");
            configurationBuilder.Properties<string>().HaveColumnType("nvarchar(200)");

            base.ConfigureConventions(configurationBuilder);
        }
    }

    public class MySqlDbContext : MainDbContext
    {
        public MySqlDbContext() { }
        public MySqlDbContext(DbContextOptions options) : base(options, null, null) { }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = SetupDatabaseConnection(optionsBuilder, 
                    (options, connectionString) => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");

            base.ConfigureConventions(configurationBuilder);

        }
    }

    public class PostgresDbContext : MainDbContext
    {
        public PostgresDbContext() { }
        public PostgresDbContext(DbContextOptions options) : base(options, null, null){ }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = SetupDatabaseConnection(optionsBuilder, 
                    (options, connectionString) => options.UseNpgsql(connectionString));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");
            base.ConfigureConventions(configurationBuilder);
        }
    }

    public class SqliteDbContext : MainDbContext
    {
        public SqliteDbContext() { }
        public SqliteDbContext(DbContextOptions options) : base(options, null, null){ }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = SetupDatabaseConnection(optionsBuilder, 
                    (options, connectionString) => options.UseSqlite(connectionString));
            }

            base.OnConfiguring(optionsBuilder);
        }
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Attraction>()
        //         .HasOne(a => a.Address)
        //         .WithMany() // many attractions can have the same address
        //         .IsRequired(false) // This makes the foreign key nullable
        //         .OnDelete(DeleteBehavior.SetNull); // This sets the foreign key to null when the parent is deleted

        // }
    }
    #endregion


    #region Methods used only during Migration
    private void DesignTimeMigrationCreateServices()
    {
        //ASP.NET Core program.cs has not run by efc design-time, configure and create services as in 
        //program.cs

        // Create a configuration
        System.Console.WriteLine($"   configuring");
        var conf = new ConfigurationBuilder();
        conf.AddApplicationSecrets("../Configuration/Configuration.csproj");

        System.Console.WriteLine($"   building configuring");
        var configuration = conf.Build();

        System.Console.WriteLine($"   creating services");
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDatabaseConnections(configuration);
        serviceCollection.AddSingleton<IConfiguration>(configuration);

        // Build the service provider and get the DbContext
        System.Console.WriteLine($"   retrieving services from serviceProvider");

        var serviceProvider = serviceCollection.BuildServiceProvider();
        _configuration = serviceProvider.GetRequiredService<IConfiguration>();
        System.Console.WriteLine($"   {nameof(IConfiguration)} retrieved");

        _databaseConnections = serviceProvider.GetRequiredService<DatabaseConnections>();
        System.Console.WriteLine($"   {nameof(DatabaseConnections)} retrieved");
        System.Console.WriteLine($"      secret source: {_databaseConnections.SetupInfo.SecretSource}");
        System.Console.WriteLine($"      database server: {_databaseConnections.SetupInfo.DataConnectionServer}");
        System.Console.WriteLine($"      database connection tag: {_databaseConnections.SetupInfo.DataConnectionTag}");

    }

    private DbConnectionDetail GetDatabaseConnection()
    {
        var connection = _databaseConnections.GetDataConnectionDetails(_configuration["DatabaseConnections:MigrationDataUser"]);
        if (connection.DbConnectionString == null)
        {
            throw new InvalidDataException($"Error: Connection string for {connection.DbConnection}, {connection.DbUserLogin} not set");
        }

        return connection;
    }

    DbContextOptionsBuilder SetupDatabaseConnection(DbContextOptionsBuilder optionsBuilder, Func<DbContextOptionsBuilder, string, DbContextOptionsBuilder> options)
    {
        System.Console.WriteLine($"{nameof(SqlServerDbContext)}: executing OnConfiguring...");
        if (_databaseConnections == null || _configuration == null)
        {
            DesignTimeMigrationCreateServices();
        }

        DbConnectionDetail connection = GetDatabaseConnection();

        optionsBuilder = options(optionsBuilder, connection.DbConnectionString);
        System.Console.WriteLine($"{nameof(SqlServerDbContext)}: OnConfiguring completed successfully");
        System.Console.WriteLine($"Proceeding with migration with user: {connection.DbUserLogin} " +
                                        $"on database connection: {connection.DbConnection}");
        return optionsBuilder;
    }
    #endregion
}


