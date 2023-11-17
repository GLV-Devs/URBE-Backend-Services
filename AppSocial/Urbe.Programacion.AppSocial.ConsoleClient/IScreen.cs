namespace Urbe.Programacion.AppSocial.ConsoleClient;

public interface IScreen
{
    public string Name { get; }

    public ValueTask RenderAsync(int height, int width);

    public IEnumerable<string> GetInputOptions();

    public ValueTask<SubmitInputResult> SubmitInput(string? input, IInputParser parser);

    public ValueTask Cleanup();
}
