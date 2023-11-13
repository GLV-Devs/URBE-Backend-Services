namespace Urbe.Programacion.AppSocial.ConsoleClient;

public class ConsoleScreenDriver
{
    private readonly Stack<IScreen> ScreenStack = new();
    private IScreen? Current;

    private int h;
    private int w;

    public async Task Run(IScreen start)
    {
        ScreenStack.Push(start);
        Current = start;

        w = Console.BufferWidth;
        h = Console.BufferHeight;

        while (true)
        {
            int y = 0;
            foreach (var inp in Current.GetInputOptions())
                RenderInputLine(inp, y++);

            RenderInputIndexIndices(y);

            var (nw, nh) = (Console.BufferWidth, Console.BufferHeight);
            if (nw != w || nh != h)
                (w, h) = (nw, nh);

            await Current.RenderAsync(h - y, w);

            var input = Console.ReadLine();
            //await Current.SubmitInput(input, this);
        }
    }

    protected virtual void RenderInputLine(string name, int lineFromBottom)
    {
        Console.SetCursorPosition(3, Console.BufferHeight - lineFromBottom - 1);
        Console.Write(name);
    }

    protected virtual void RenderInputIndexIndices(int indeces)
    {
        var y = Console.BufferHeight - 1 - indeces;
        for (int i = 0; i < indeces; i++)
        {
            Console.SetCursorPosition(0, y - i);
            Console.Write(i);
            Console.Write(": ");
        }
    }
}
