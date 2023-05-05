﻿using Microsoft.EntityFrameworkCore;
using Vendor.Services.Machines.Domain.AggregateModel.HandleAggregate;
using Vendor.Services.Machines.Domain.AggregateModel.MachineAggregate;
using Vendor.Services.Machines.Domain.SeedWork;

namespace Vendor.Services.Machines.Infrastructure;

public class MachineDbContext : DbContext, IUnitOfWork
{
    public DbSet<Vending> Vendings { get; set; }
    public DbSet<Spiral> Spirals { get; set; }

    public DbSet<Handle> Handles { get; set; }
    public DbSet<Servicing> Servicings { get; set; }

    public MachineDbContext()
    {
        
    }

    public MachineDbContext(DbContextOptions<MachineDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}