using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.ThemeManager;

namespace BeerDrivenFrontend.Client.Shared;

public class MainLayoutBase : LayoutComponentBase, IDisposable
{
    protected bool DrawerOpen = true;
    protected ThemeManagerTheme ThemeManager = new();
    public bool ThemeManagerOpen;

    protected int counter = 0;
    protected string data = string.Empty;
    private HubConnection? hubConnection;
    private string messages = string.Empty;
    private string username = string.Empty;
    private string message = string.Empty;
    ElementReference TextAreaRef;

    public record Count(int Value, DateTime Data, string Nome, string Cognome);
    private async Task Connect()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:7043/chathub")
            .WithAutomaticReconnect()
            .Build();


        //hubConnection.On<int>("SetCounter", count =>
        //{
        //    Console.WriteLine(count);
        //    counter = count;
        //    StateHasChanged();
        //});

        hubConnection.On<Count>("SetCounter", count =>
        {
            Console.WriteLine(count);
            counter = count.Value;
            data = count.Data.ToString();
            StateHasChanged();
        });


        //hubConnection.On<string, string>("GetThatMessageDude", (user, message) =>
        //{
        //    var msg = $"{(string.IsNullOrEmpty(user) ? "" : user + ": ")}{message}";
        //    messages += msg + "\n";
        //    JSRuntime.InvokeVoidAsync("scrollToBottom", TextAreaRef);
        //    StateHasChanged();
        //});

        await hubConnection.StartAsync();
    }

    private async Task Send()
    {
        if (hubConnection != null)
        {
            await hubConnection.SendAsync("AddMessageToChat", username, message);
            message = string.Empty;
        }
    }

    private async Task HandleInput(KeyboardEventArgs args)
    {
        if (args.Key.Equals("Enter"))
        {
            await Send();
        }
    }

    public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

    public async ValueTask DisposeAsync()
    {
        if (hubConnection != null)
        {
            await hubConnection.DisposeAsync();
        }
    }



    protected override void OnInitialized()
    {
        StateHasChanged();
        Connect();
    }

    protected void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }

    protected void OpenThemeManager(bool value)
    {
        ThemeManagerOpen = value;
    }

    protected void UpdateTheme(ThemeManagerTheme value)
    {
        ThemeManager = value;
        StateHasChanged();
    }

    #region Dispose
    private void Dispose(bool disposing)
    {
        if (hubConnection != null)
        {
            hubConnection.DisposeAsync();
        }
    }
    public void Dispose()
    {
        if (hubConnection != null)
        {
            hubConnection.DisposeAsync();
        }
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~MainLayoutBase()
    {
        Dispose(false);
    }
    #endregion
}