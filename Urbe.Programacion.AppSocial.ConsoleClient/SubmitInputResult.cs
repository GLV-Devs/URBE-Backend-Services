namespace Urbe.Programacion.AppSocial.ConsoleClient;

public readonly record struct SubmitInputResult
{
    public enum InputAction
    {
        Nothing,
        NextScreen,
        PreviousScreen,
        InvalidInput
    }

    public IScreen? Next { get; private init; }
    public InputAction Action { get; private init; }

    public static SubmitInputResult ChangeScreen(IScreen screen)
    {
        ArgumentNullException.ThrowIfNull(screen);
        return new()
        {
            Action = InputAction.NextScreen,
            Next = screen
        };
    }

    public static SubmitInputResult DoNothing => default;
    public static SubmitInputResult PreviousScreen => new() { Action = InputAction.PreviousScreen };
    public static SubmitInputResult InvalidInput => new() { Action = InputAction.InvalidInput };
}
