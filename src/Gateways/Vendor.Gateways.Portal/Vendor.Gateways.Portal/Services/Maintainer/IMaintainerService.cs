﻿using Vendor.Domain.Types;
using Vendor.Domain.Views;

namespace Vendor.Gateways.Portal.Services.Maintainer;

public interface IMaintainerService
{
    Task<ApiResponse<List<VendingView>>> FetchEmptyMachines();

    Task HandleMachine(string title);
}