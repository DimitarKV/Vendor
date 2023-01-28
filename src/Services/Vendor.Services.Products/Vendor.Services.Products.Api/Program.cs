using Vendor.Application.Extensions;
using Vendor.Domain.Commands.UploadImageCommand;
using Vendor.Services.Products.Api.Controllers;
using Vendor.Services.Products.Data.Extensions;
using Vendor.Services.Products.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddPersistence();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(new [] {typeof(ProductsController).Assembly, typeof(ProductProfiles).Assembly, typeof(UploadImageCommand).Assembly});
builder.AddSecurity();
builder.AddCloudinary();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.EnsureDatabaseCreated();
app.UseSecurity();
app.MapControllers();

app.Run();