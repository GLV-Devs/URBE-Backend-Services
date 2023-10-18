namespace Urbe.BasesDeDatos.AppSocial.HTTPModels;

public enum APIResponseCodeEnum : long
{
    Success = 1,
    Empty = 0,
    ErrorCollection = -1,
    Exception = -2
}

public readonly record struct APIResponseCode(APIResponseCodeEnum ResponseId) 
{
    public string Name { get; } = ResponseId.ToString();

    public static implicit operator APIResponseCode(APIResponseCodeEnum code)
        => new(code);
}
