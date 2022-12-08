using Vendor.Application.Extensions;
using Vendor.Services.Machines.Api.Controllers;
using Vendor.Services.Machines.Data.Entities;
using Vendor.Services.Machines.Data.Extensions;
using Vendor.Services.Machines.Data.SynchronousConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddPersistence();
builder.AddHttpClients();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(new[] {typeof(Machine).Assembly, typeof(VendingController).Assembly});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.EnsureDatabaseCreated();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();