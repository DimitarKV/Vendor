﻿using Vendor.Domain.Views;

namespace Vendor.Gateways.Portal.Services;

public interface IMaintainerService
{
    Task<List<VendingView>> FetchEmptyMachines();

    Task HandleMachine(string title);
}