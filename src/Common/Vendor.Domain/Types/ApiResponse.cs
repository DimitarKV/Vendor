namespace Vendor.Domain.Types;

public class ApiResponse
{
    public List<string>? Errors { get; init; }
    public string Message { get; set; }
    
    public ApiResponse(string message, IEnumerable<string>? errors = null)
    {
        Message = message;
        Errors = errors?.ToList();
    }

    public ApiResponse()
    {
        Errors = new List<string>();
        Message = "";
    }

    public bool IsValid => (Errors == null || !Errors.Any());
}

public class ApiResponse<TModel> : ApiResponse
{
    public TModel Result { get; init; }
    
    public ApiResponse(TModel model, string message = "", IEnumerable<string>? errors = null) 
        : base(message, errors)
    {
        Result = model;
    }

    public ApiResponse()
    {
        
    }
}