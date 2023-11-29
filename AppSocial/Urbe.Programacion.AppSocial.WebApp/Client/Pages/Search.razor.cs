using System.Diagnostics;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Pages;

public partial class Search
{
    private static readonly TimeSpan InputDelay = TimeSpan.FromSeconds(3);
    private readonly object sync = new();

    private bool isLoading;
    public bool IsLoading
    {
        get => isLoading;
        set
        {
            if (value != isLoading)
            {
                isLoading = value;
                StateHasChanged();
            }
        }
    }

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

        ResultsVisible = false;
        IsLoading = true;
        await Task.Yield();
        try
        {
            var resp = await Client.Users.GetUsers(search);
            if (CheckResponse(resp, APIResponseCodeEnum.UserView) is false)
                return;

            Debug.Assert(resp.APIResponse?.Data is not null);
            UserResults = resp.APIResponse.Data.Cast<UserViewModel>();
            ResultsVisible = true;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
