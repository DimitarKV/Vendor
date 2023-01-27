FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Vendor.Services.User/Vendor.Services.User.Api/Vendor.Services.User.Api.csproj", "Vendor.Services.User.Api/"]
RUN dotnet restore "Services/Vendor.Services.User/Vendor.Services.User.Api/Vendor.Services.User.Api.csproj"
COPY . .
WORKDIR "/src/Vendor.Services.User.Api"
RUN dotnet build "Vendor.Services.User.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vendor.Services.User.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vendor.Services.User.Api.dll"]
