using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using EcommerceProject.Context;
using EcommerceProject.Repositories;
using EcommerceProject.Services;
using EcommerceProject.Models.Configuration;
using EcommerceProject.Services.Payment;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlString"));
});
builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddScoped<OrdenRepository>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<OrdenService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<PaymentService, TransbankPaymentService>();

builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuentas/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        //options.LogoutPath = "/Usuario/Logout";
        options.AccessDeniedPath = "/Home/Error";
    });
builder.Services.Configure<TransbankSettings>( builder.Configuration.GetSection("Transbank"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
