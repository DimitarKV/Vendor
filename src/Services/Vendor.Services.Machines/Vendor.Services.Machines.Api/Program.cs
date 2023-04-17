using Vendor.Application.Extensions;
using Vendor.Services.Machines.AggregateModel.MachineAggregate;
using Vendor.Services.Machines.Api;
using Vendor.Services.Machines.Api.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddPersistence();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(new[] {typeof(Machine).Assembly, typeof(VendingController).Assembly});
builder.AddSecurity();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.EnsureDatabaseCreated();
app.MapControllers();
app.UseSecurity();

app.Run();