using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services.Data;

public class AppConfigurationDbContext : ConfigurationDbContext<AppConfigurationDbContext>
{
    // Asset
    public AppConfigurationDbContext(
        DbContextOptions<AppConfigurationDbContext> options)
        : base(options)
    {

    }
}