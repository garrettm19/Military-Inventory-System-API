using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Military_Inventory_System_API.Context;

var builder = WebApplication.CreateBuilder(args);

// Add configuration sources
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
.Build();

builder.Services.AddDbContext<MilitaryInventorySystemContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConn"));
});

builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors("AllowLocalhost3000");

app.UseAuthorization();

app.MapControllers();

app.Run();