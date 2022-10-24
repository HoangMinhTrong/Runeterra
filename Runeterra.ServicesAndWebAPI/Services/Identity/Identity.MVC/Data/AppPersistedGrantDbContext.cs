using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services.Data;

public class AppPersistedGrantDbContext : PersistedGrantDbContext<AppPersistedGrantDbContext>
{
    // Asset
    public AppPersistedGrantDbContext(
        DbContextOptions<AppPersistedGrantDbContext> options)
        : base(options)
    {

    }
}