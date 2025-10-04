using IdleCore.Model;
using IdleDB.Converters;
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

    public DbSet<Character> Characters { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Account>(b =>
        {
            b.HasKey(a => a.Id);
            b.HasOne<IdentityUser>().WithOne().HasPrincipalKey<IdentityUser>(a => a.Id).HasForeignKey<Account>(u => u.Id).IsRequired();
        });

        builder.Entity<Character>(b =>
        {
            b.HasKey(c => c.Id);
            b.HasOne(c => c.Account).WithMany(a => a.Characters).HasPrincipalKey(a => a.Id).HasForeignKey(c => c.AccountId).IsRequired();
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeValueConverter>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<NullableDateTimeValueConverter>();
    }
}
