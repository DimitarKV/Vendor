using Vendor.Application.Extensions;
using Vendor.Services.User.Api.Controllers;
using Vendor.Services.User.Data.Entities;
using Vendor.Services.User.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddPersistence();
builder.Services.AddApplication(new[] {typeof(VendorUser).Assembly, typeof(UserController).Assembly});
builder.AddIdentity();
builder.AddSecurity();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.EnsureDatabaseCreated();

app.UseSecurity();
app.MapControllers();

app.Run();