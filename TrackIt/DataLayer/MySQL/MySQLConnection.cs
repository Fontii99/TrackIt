namespace TrackIt.DataLayer.MySQL;

using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;

public interface IMySQLConnection
{
    string GetConnectionString();
    MySqlConnection CreateConnection();
}

public class MySQLConnection : IMySQLConnection
{
    private readonly IConfiguration _configuration;

    // Constructor for dependency injection
    public MySQLConnection(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string GetConnectionString()
    {
        var server = _configuration["ConnectionStrings:Server"];
        var database = _configuration["ConnectionStrings:Database"];
        var user = _configuration["ConnectionStrings:User"];
        var password = _configuration["ConnectionStrings:Password"];

        // Check for null values and provide a helpful error message
        if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(database) ||
            string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException("Database connection parameters are missing in the configuration file.");
        }

        return $"Server={server};Database={database};User={user};Password={password};";
    }

    public MySqlConnection CreateConnection()
    {
        var connectionString = GetConnectionString();
        return new MySqlConnection(connectionString);
    }
}