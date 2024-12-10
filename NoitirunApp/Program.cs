using Microsoft.EntityFrameworkCore;
using NoitirunApp.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Noitirun.Core.Interfaces;
using Noitirun.EF.Repositories;
using Noitirun.Core.Mapper;
using Commun.Application.seedWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "NoitirunApp",
        Version = "v1",
        Description = "Api Documentation for NoitirunApp",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Email = "moahamedmalek.douik@gmail.com",
            Url = new Uri("https://yourwebsite.com")
        },
    });

    // Add XML Comments (Optional)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
});

// DBContext
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
// EndDBContext

// ADD MediatR : Since MediatR 12.0.0
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// ADD Repositories
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

// ADD Mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ADD Interface
builder.Services.AddTransient<IDateTimeService, DateTimeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
