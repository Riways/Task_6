using Task_6_Blazor_Server.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Task_6_Blazor_Server.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Task_6_Blazor_Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString), ServiceLifetime.Scoped);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IMessageService,MessageService>();
builder.Services.AddMudServices();
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults
    .MimeTypes.Concat(new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<MessageHub>("/messagehub");
app.MapFallbackToPage("/_Host");

app.Run();
