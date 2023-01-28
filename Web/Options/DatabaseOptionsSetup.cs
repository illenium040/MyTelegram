using Microsoft.Extensions.Options;

namespace Web.Options;

public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private const string ConfigurationDatabaseSection = "DatabaseOptions";
    private readonly IConfiguration _configuration;
    public DatabaseOptionsSetup(IConfiguration configuration) => _configuration = configuration;
    public void Configure(DatabaseOptions options)
    {
        options.ConnectionString = _configuration.GetConnectionString("LocalDatabase");
        _configuration.GetSection(ConfigurationDatabaseSection).Bind(options);
    }
}
