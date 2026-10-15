using Microsoft.EntityFrameworkCore;
using StockManagementAPI.Data;
using StockManagementAPI.Exceptions;
using StockManagementAPI.Repositories;
using StockManagementAPI.Services;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register AutoMapper
// Register AutoMapper profiles by assembly
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register repositories and services
builder.Services.AddScoped<StockManagementAPI.Interfaces.IBranchRepository, BranchRepository>();
builder.Services.AddScoped<StockManagementAPI.Interfaces.IBranchService,BranchService>();

builder.Services.AddScoped<StockManagementAPI.Interfaces.IProductRepository, ProductRepository>();
builder.Services.AddScoped<StockManagementAPI.Interfaces.IProductService,ProductService>();

builder.Services.AddScoped<StockManagementAPI.Interfaces.IRequestRepository, RequestRepository>();
builder.Services.AddScoped<StockManagementAPI.Interfaces.IRequestService,RequestService>();

builder.Services.AddScoped<StockManagementAPI.Interfaces.IRequestStatusRepository, RequestStatusRepository>();
builder.Services.AddScoped<StockManagementAPI.Interfaces.IRequestStatusService, RequestStatusService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
