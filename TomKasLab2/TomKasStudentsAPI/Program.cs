using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TomKasStudentsAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TomKasStudentsAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TomKasStudentsAPIContext") ?? throw new InvalidOperationException("Connection string 'TomKasStudentsAPIContext' not found.")));

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;
SeedData.Initialize(services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
