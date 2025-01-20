using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Configuration;

public enum DatabaseServer {SQLServer, MySql, PostgreSql, SQLite}
public class DatabaseConnections
{
    readonly IConfiguration _configuration;
    readonly DbConnectionSetsOptions _options;
    private readonly DbSetDetailOptions _activeDataSet;
 
    public SetupInformation SetupInfo => new SetupInformation () {

            SecretSource = _configuration.GetValue<bool>("ApplicationSecrets:UseAzureKeyVault")
                ?$"Azure: {Environment.GetEnvironmentVariable("AZURE_KeyVaultSecret")}"
                :$"Usersecret: {Environment.GetEnvironmentVariable("USERSECRETID")}",
            
            DefaultDataUser = _configuration["DatabaseConnections:DefaultDataUser"],
            MigrationDataUser = _configuration["DatabaseConnections:MigrationDataUser"],
            
            DataConnectionTag = _activeDataSet.DbTag,
            
            DataConnectionServer = _activeDataSet.DbServer.Trim().ToLower() switch {
                "sqlserver" => DatabaseServer.SQLServer,    
                "mysql" => DatabaseServer.MySql,
                "postgresql" => DatabaseServer.PostgreSql,
                "sqlite" => DatabaseServer.SQLite,
                _ => throw new NotSupportedException ($"DbServer {_activeDataSet.DbServer} not supported")},

            };

    public DbConnectionDetail GetDataConnectionDetails (string user) => GetLoginDetails(user, _activeDataSet);

    DbConnectionDetail GetLoginDetails(string user, DbSetDetailOptions dataSet)
    {
        if (string.IsNullOrEmpty(user) || string.IsNullOrWhiteSpace(user))
            throw new ArgumentNullException(nameof(user));

        var conn = dataSet.DbConnections.First(m => m.DbUserLogin.Trim().ToLower() == user.Trim().ToLower());
        return new DbConnectionDetail
        {
            DbUserLogin = conn.DbUserLogin,
            DbConnection = conn.DbConnection,
            DbConnectionString = _configuration.GetConnectionString(conn.DbConnection)
        };
    }

    //Not to revieal the connection string, I find the corresponding DbConnectionKey in the 
    public string GetDbConnection (string DbConnectionString) => 
            _activeDataSet.DbConnections.First(m =>_configuration.GetConnectionString(m.DbConnection).Trim().ToLower() == DbConnectionString.Trim().ToLower())?.DbConnection;



    public DatabaseConnections(IConfiguration configuration, IOptions<DbConnectionSetsOptions> dbSetOption)
    {
        _configuration = configuration;
        _options = dbSetOption.Value;

        _activeDataSet = _options.DataSets.FirstOrDefault(ds => ds.DbTag.Trim().ToLower() == configuration["DatabaseConnections:UseDataSetWithTag"].Trim().ToLower());
        if (_activeDataSet == null) 
            throw new ArgumentException($"Dataset with DbTag {configuration["DatabaseConnections:UseDataSetWithTag"]} not found");
    }


    public class SetupInformation
    {
        public string AppEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public string SecretSource {get; init;}
        public string DataConnectionTag {get; init;}
        public string DefaultDataUser {get; init;}
        public string MigrationDataUser {get; init;}
        public DatabaseServer DataConnectionServer {get; init;}
        public string DataConnectionServerString => DataConnectionServer.ToString();  //for json clear text
    }
}