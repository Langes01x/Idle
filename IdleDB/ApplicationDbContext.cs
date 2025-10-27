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

    public DbSet<Party> Parties { get; set; } = default!;

    public DbSet<Area> Areas { get; set; } = default!;

    public DbSet<Level> Levels { get; set; } = default!;

    public DbSet<Enemy> Enemies { get; set; } = default!;

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
            b.HasOne<Account>().WithMany(a => a.Characters).HasPrincipalKey(a => a.Id).HasForeignKey(c => c.AccountId).IsRequired();
        });

        builder.Entity<Party>(b =>
        {
            b.HasKey(p => p.Id);
            b.HasOne(p => p.BackCharacter).WithMany().HasPrincipalKey(c => c.Id).HasForeignKey(p => p.BackCharacterId);
            b.HasOne(p => p.MiddleCharacter).WithMany().HasPrincipalKey(c => c.Id).HasForeignKey(p => p.MiddleCharacterId);
            b.HasOne(p => p.FrontCharacter).WithMany().HasPrincipalKey(c => c.Id).HasForeignKey(p => p.FrontCharacterId);
            b.HasOne<Account>().WithMany(a => a.Parties).HasPrincipalKey(a => a.Id).HasForeignKey(p => p.AccountId).IsRequired();
        });

        builder.Entity<Enemy>(b =>
        {
            b.HasKey(e => e.Id);
        });

        builder.Entity<Level>(b =>
        {
            b.HasKey(l => l.Id);
            b.HasMany(l => l.Enemies).WithOne().HasPrincipalKey(l => l.Id).HasForeignKey(e => e.LevelId).IsRequired();
        });

        builder.Entity<Area>(b =>
        {
            b.HasKey(a => a.Id);
            b.HasMany(a => a.Levels).WithOne().HasPrincipalKey(a => a.Id).HasForeignKey(l => l.AreaId).IsRequired();
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeValueConverter>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<NullableDateTimeValueConverter>();
    }
}
