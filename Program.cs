using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using quiz_project.Controllers;
using quiz_project.Database;
using quiz_project.Entities;
using quiz_project.Entities.Repositories;
using quiz_project.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connection = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' was not found");
builder.Services.AddDbContext<QuizDb>(o => o.UseSqlite(connection));
builder.Services.AddScoped<IQuizRepository, QuizRepository>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id:int?}");

app.MapControllerRoute(
    name: "fallback",
    pattern: "{*Home}",
    defaults: new { controller = "Home", action = "Index" });

using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<QuizDb>();
    DbSeeder.Initialize(context);
}

app.Run();
