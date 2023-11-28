using System.Diagnostics;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Components;

public partial class SearchBar
{
    private static readonly TimeSpan InputDelay = TimeSpan.FromSeconds(3);
    private readonly object sync = new();

    public string? SearchQuery { get; set; }

    public IEnumerable<UserViewModel>? UserResults { get; set; }

    public bool ResultsVisible { get; set; }

    private Task? InputWaiter;
    private DateTime InputTimer;
    private void HandleInput()
    {
        InputTimer = DateTime.Now;
        if (InputWaiter is null)
            lock (sync)
                InputWaiter ??= Task.Run(WaitForInput);
    }

    private async Task WaitForInput()
    {
        while (true)
        {
            if (DateTime.Now - InputTimer > InputDelay)
            {
                await FetchResults();
                InputWaiter = null;
                return;
            }
            await Task.Delay(1000);
        }
    }

    protected async Task FetchResults()
    {
        var search = SearchQuery;
        if (string.IsNullOrWhiteSpace(search))
            return;

        var resp = await Client.Users.GetUsers(search);
        if (CheckResponse(resp, APIResponseCodeEnum.UserView) is false)
            return;

        Debug.Assert(resp.APIResponse?.Data is not null);
        UserResults = resp.APIResponse.Data.Cast<UserViewModel>();
        ResultsVisible = true;
    }
}
