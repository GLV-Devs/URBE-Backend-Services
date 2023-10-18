namespace Urbe.BasesDeDatos.AppSocial.ModelServices.API.Responses;

public enum APIResponseCodeEnum : int
{
    UserView = 10,

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
