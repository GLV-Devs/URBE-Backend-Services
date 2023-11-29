using System.Diagnostics;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Pages;

public partial class Search
{
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

    protected async Task FetchResults()
    {
        var search = SearchQuery;
        if (string.IsNullOrWhiteSpace(search))
            return;

        try
        {
            IsLoading = true;
            await Task.Yield();

            var resp = await Client.Users.GetUsers(search);

            Debug.Assert(resp.APIResponse?.Data is not null);
            UserResults = resp.APIResponse.Data.Cast<UserViewModel>();
            StateHasChanged();
        }
        finally
        {
            IsLoading = false;
        }
    }
}
