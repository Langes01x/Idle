using IdleCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdleDB;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Account>(b =>
        {
            b.HasKey(a => a.Id);
            b.HasOne<IdentityUser>().WithOne().HasPrincipalKey<IdentityUser>(a => a.Id).HasForeignKey<Account>(u => u.Id).IsRequired();
        });
    }
}
