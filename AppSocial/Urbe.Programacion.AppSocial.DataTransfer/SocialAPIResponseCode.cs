using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.DataTransfer;

public enum APIResponseCodeEnum : int
{
    PostView = 20,

    UserSelfView = 11,
    UserView = 10,

    NoData = 2,
    Success = 1,

    Empty = 0,

    ErrorCollection = -1,
    Exception = -2,
    UnspecifiedError = -3
}

public readonly record struct SocialAPIResponseCode(APIResponseCodeEnum ResponseId) : IAPIResponseObjectCode<SocialAPIResponseCode>
{
    public string Name { get; } = ResponseId.ToString();

    public static implicit operator SocialAPIResponseCode(APIResponseCodeEnum code)
        => new(code);

    public static SocialAPIResponseCode NoData => APIResponseCodeEnum.NoData;
    public static SocialAPIResponseCode ErrorCollection => APIResponseCodeEnum.ErrorCollection;
    public static SocialAPIResponseCode Success => APIResponseCodeEnum.Success;
    public static SocialAPIResponseCode UnspecifiedError => APIResponseCodeEnum.UnspecifiedError;
    public static SocialAPIResponseCode Exception => APIResponseCodeEnum.Exception;
}

public static class SocialAPIResponseCodeExtensions
{
    public static bool IsExpectedResponse(this SocialAPIResponseCode code, ref ErrorList errors, APIResponseCodeEnum expected)
    {
        if (code != expected)
        {
            errors.AddError(ErrorMessages.UnexpectedServerResponse((int)code.ResponseId, code.Name));
            return false;
        }

        return true;
    }
}