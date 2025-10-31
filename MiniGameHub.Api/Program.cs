using Duende.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniGameHub.Api.Configuration;
using MiniGameHub.Api.Data;
using MiniGameHub.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// IdentityServer
builder.Services.AddIdentityServer(options =>
{
    options.KeyManagement.Enabled = false;
})
.AddAspNetIdentity<IdentityUser>() 
.AddInMemoryClients(Config.Clients)
.AddInMemoryIdentityResources(Config.IdentityResources)
.AddInMemoryApiScopes(Config.ApiScopes)
.AddDeveloperSigningCredential();


builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "X-CSRF-TOKEN-MINIGAME";
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
    options.DefaultChallengeScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
})
.AddCookie("Cookies", options =>
{
    options.Cookie.Name = "idsrv.cookie";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.LoginPath = "/Account/Login";
})
.AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:5093";
    options.Audience = "minigames_api";
    options.RequireHttpsMetadata = false;
});

var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())    
{
    app.MapOpenApi();
}


app.UseCors(myAllowSpecificOrigins);
app.UseIdentityServer();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.MapHub<StopHub>("/stophub");
app.MapControllers();

app.Run();
