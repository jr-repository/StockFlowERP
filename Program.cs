using StockFlowERP.Repositories;
using StockFlowERP.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton(typeof(IJsonRepository<>), typeof(JsonRepository<>));
builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<PartyService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<SalesService>();
builder.Services.AddScoped<DashboardService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("DefaultCors");
app.UseAuthorization();

app.MapGet("/", () => new
{
    name = "StockFlow ERP API",
    version = "1.0.0",
    status = "Running"
});

app.MapControllers();

app.Run();
