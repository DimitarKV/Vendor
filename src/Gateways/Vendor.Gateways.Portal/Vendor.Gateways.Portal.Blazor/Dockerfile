FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Gateways/Vendor.Gateways.Portal.Blazor/Vendor.Gateways.Portal.Blazor.csproj", "Vendor.Gateways.Portal.Blazor/"]
RUN dotnet restore "Gateways/Vendor.Gateways.Portal.Blazor/Vendor.Gateways.Portal.Blazor.csproj"
COPY . .
WORKDIR "/src/Vendor.Gateways.Portal.Blazor"
RUN dotnet build "Vendor.Gateways.Portal.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vendor.Gateways.Portal.Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vendor.Gateways.Portal.Blazor.dll"]
