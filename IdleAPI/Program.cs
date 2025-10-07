using IdleAPI.Converters;
using IdleCore.Helpers;
using IdleDB;
using IdleDB.Helpers;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Identity support
builder.Services.AddAuthorization();
// Should enable the following for a production application with an email sender implementation
// options => options.SignIn.RequireConfirmedAccount = true
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<IdentityOptions>(o =>
{
    // Ensure emails are unique
    o.User.RequireUniqueEmail = true;

    o.SignIn.RequireConfirmedAccount = false;
    o.Tokens.AuthenticatorIssuer = "IdleAPI";
});

// Fix JSON body conversion of true / false string values to booleans
builder.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.Converters.Add(new BooleanConverter());
    o.SerializerOptions.Converters.Add(new NullableBooleanConverter());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAccountManager, AccountManager>();
builder.Services.AddScoped<ICharacterManager, CharacterManager>();
builder.Services.AddScoped<ISummonHelper, SummonHelper>();
builder.Services.AddScoped<ILevelCalculator, LevelCalculator>();
builder.Services.AddScoped<IStatCalculator, StatCalculator>();
builder.Services.AddScoped<ICollectionHelper, CollectionHelper>();
builder.Services.AddScoped<ICharacterSorter, CharacterSorter>();
builder.Services.AddScoped<IEnumMapper, EnumMapper>();
builder.Services.AddScoped<Random>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<IdentityUser>();

app.Run();
