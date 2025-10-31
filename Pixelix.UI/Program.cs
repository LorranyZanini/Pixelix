using Pixelix.UI.Middleware;
using Pixelix.UI.Models;
using Pixelix.UI.Services.Implementations;
using Pixelix.UI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configurações
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddScoped<UserContextService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuração da Autenticação
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "SessionAuth";
    options.DefaultChallengeScheme = "SessionAuth";
    options.DefaultSignInScheme = "SessionAuth";
})
.AddCookie("SessionAuth", options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AcessoNegado";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => 
        policy.RequireRole("Administrador"));
    
    options.AddPolicy("Gerente", policy => 
        policy.RequireRole("Gerente"));
    
    options.AddPolicy("Cliente", policy => 
        policy.RequireRole("Cliente"));
});

builder.Services.AddHttpContextAccessor();

// Serviços de API
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.Use(async (context, next) =>
{
    var userContextService = context.RequestServices.GetRequiredService<UserContextService>();
    context.User = userContextService.CreateClaimsPrincipal();
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();