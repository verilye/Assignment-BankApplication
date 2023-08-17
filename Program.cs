using WebDevAss2.Data.Repositories;
using WebDevAss2.Models;
using WebDevAss2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<McbaDbContext>(options => options.UseSqlServer("Server=rmit.australiaeast.cloudapp.azure.com;Database=s3768929_a2;uid=s3768929_a2;Password=abc123;TrustServerCertificate=True;"),ServiceLifetime.Scoped);
builder.Services.AddScoped<IUserDataWebServiceRepository<List<Customer>>, UserDataWebServiceRepository<List<Customer>>>();
builder.Services.AddScoped<IDataAccessRepository, DataAccessRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddHttpClient<UserDataWebServiceRepository<List<Customer>>>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddHostedService<ScheduledPaymentService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options=>{
        options.LoginPath = "/Login";
        options.LogoutPath = "/Login";
    });

builder.Services.AddSession(options=>{
    options.Cookie.Name ="WebDevAss2.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "home",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
