using System.Net.WebSockets;
using System.Text.Json.Serialization;

namespace PlaceBot;

public class Account
{
    public int Index { get; set; }
    
    public string AccessToken { get; }

    [JsonIgnore]
    public ClientWebSocket Client { get; set; }

    public Account(string token)
    {
        AccessToken = token;
    }
    
    public Account()
    {

    }

    public async Task InitializeAsync()
    {
        Client = new ClientWebSocket();
        await Client.ConnectAsync(new Uri("wss://server.rplace.tk:1291"), CancellationToken.None);
    }
}