namespace PlaceBot;

public class Account
{
    public uint Index { get; set; }
    
    public string AccessToken { get; }

    public HttpClient Client { get; }

    public Account(string token)
    {
        AccessToken = token;
        /*Client = new HttpClient();
        Client.DefaultRequestHeaders.Add("origin", "https://hot-potato.reddit.com");
        Client.DefaultRequestHeaders.Add("referer", "https://hot-potato.reddit.com/");
        Client.DefaultRequestHeaders.Add("apollographql-client-name", "mona-lisa");
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");
        Client.DefaultRequestHeaders.Add("Content-Type", "application/json");*/
    }
}