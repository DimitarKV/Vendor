﻿namespace Vendor.Services.Machines.DTO;

public class CreateVendingDto
{
    public string Title { get; set; }
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public int Spirals { get; set; }
}