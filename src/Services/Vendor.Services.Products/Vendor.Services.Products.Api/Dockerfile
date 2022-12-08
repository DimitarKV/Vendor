FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Vendor.Services.Products/Vendor.Services.Products.Api/Vendor.Services.Products.Api.csproj", "Vendor.Services.Products.Api/"]
RUN dotnet restore "Services/Vendor.Services.Products/Vendor.Services.Products.Api/Vendor.Services.Products.Api.csproj"
COPY . .
WORKDIR "/src/Vendor.Services.Products.Api"
RUN dotnet build "Vendor.Services.Products.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vendor.Services.Products.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vendor.Services.Products.Api.dll"]
