namespace Urbe.Programacion.AppSocial.ConsoleClient;

public record ClientConfig(string APIHost)
{
    public void Validate()
    {
        if (APIHost is null)
            throw new InvalidOperationException("APIHost cannot be null");
    }
}
