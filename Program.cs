using DotNetEnv;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using MinAudiovisual.Servicios.Application.Interfaces;
using MinAudiovisual.Diary.Application.Interfaces;
using MinAudiovisual.Diary.Application.Services;
using MinAudiovisual.Components;
using MinAudiovisual.Diary.Infrastructure.Interfaces;
using MinAudiovisual.Diary.Infrastructure.Repositories;
using MinAudiovisual.Migrations;
using MinAudiovisual.Servicios.Application.UseCases.Servicios;
using MinAudiovisual.Servicios.Infrastructure.Persistence.Repositories;

// using MinAudiovisual.Application.Interfaces;
// using MinAudiovisual.Application.UseCases;
// using MinAudiovisual.Infrastructure.Persistence;
// using MinAudiovisual.Infrastructure.Persistence.Repositories;
// using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddControllers();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        [ "application/octet-stream" ]);
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=MinAudiovisual.db"));

builder.Services.AddScoped<IServicioRepository, ServicioRepository>();
builder.Services.AddScoped<CreateServicioUseCase>();
builder.Services.AddScoped<IDevocionalRepository, DevocionalRepository>();
builder.Services.AddScoped<IDevocionalPdfService, DevocionalPdfService>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5250/")
});


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapControllers(); 

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();