using System.Net.WebSockets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using quiz_project.Controllers;
using quiz_project.Database;
using quiz_project.Entities;
using quiz_project.Entities.Repositories;
using quiz_project.Helpers;
using quiz_project.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Adding all services through ServiceConfigurer helper class
var finishedBuilder = ServiceConfigurer.Configure(builder);
var app = finishedBuilder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id:int?}");

// Adding fallback to routing table
app.MapControllerRoute(
    name: "fallback",
    pattern: "{*Home}",
    defaults: new { controller = "Home", action = "Index" });

// Seeding database if data is not available
using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<QuizDb>();
    DbSeeder.Initialize(context);
}

app.Run();
