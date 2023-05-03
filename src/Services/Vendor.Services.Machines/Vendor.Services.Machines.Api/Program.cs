using Vendor.Application.Extensions;
using Vendor.Domain.Commands.Cloudinary;
using Vendor.Services.Machines.Api;
using Vendor.Services.Machines.Api.Controllers;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddPersistence();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(new[] {typeof(Machine).Assembly, typeof(VendingController).Assembly, typeof(UploadImageCommand).Assembly});
builder.AddSecurity();
builder.AddCloudinary();

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