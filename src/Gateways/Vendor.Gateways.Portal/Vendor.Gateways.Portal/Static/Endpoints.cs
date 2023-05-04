namespace Vendor.Gateways.Portal.Static;

public static class Endpoints
{
    public const string QueryEmptyVendings = "/vending/queryempty";
    public const string RegisterUser = "/user/register";
    public const string Login = "/user/login";
    public const string QueryProductById = "/products/query";

    public const string HandleVending = "/vending/handlemachine";
    
    public const string EmailConfirmation = "/user/confirmemail";
    public const string QueryUsers = "/user/query";
    public const string QueryAvailableRoles = "/user/queryroles";
    public const string CreateMachine = "/vending/create";
    public const string SetMachineImageEndpoint = "/vending/setimage";
    public const string RegisterProductEndpoint = "/products/create";
    public const string FetchMachineById = "/vending/fetch";
    public const string QueryMissingProductsEndpoint = "/vending/QueryMissingProducts";
    public const string QueryProductsMyMatch = "/products/querymatching";
}