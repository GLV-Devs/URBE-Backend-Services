using System.Text.Json.Serialization;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.DataTransfer;

public class BearerAPIResponse : APIResponse<SocialAPIResponseCode>
{
    public BearerAPIResponse(SocialAPIResponseCode code) : base(code) { }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BearerToken { get; set; }
}
