namespace Urbe.Programacion.AppSocial.WebApp.Server.Options;

public class SmtpSettings
{
    public string Domain { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public string FromAddress { get; init; }
    public string FromName { get; init; }
    public ushort Port { get; init; }
    public bool UseSsl { get; init; }

    public SmtpSettings(string domain, string userName, string password, string fromAddress, string fromName, ushort port, bool useSsl)
    {
        Domain = domain ?? throw new ArgumentNullException(nameof(domain));
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        FromAddress = fromAddress ?? throw new ArgumentNullException(nameof(fromAddress));
        FromName = fromName ?? throw new ArgumentNullException(nameof(fromName));
        Port = port;
        UseSsl = useSsl;
    }
}
