namespace BFF.Communications;

public interface ICommunicationServiceClient
{
    Task SendAccountCreationConfirmationAsync(AccountCreationConfirmation confirmation);
}

public class CommunicationServiceClient : ICommunicationServiceClient{
    private readonly HttpClient _client;

    public CommunicationServiceClient(HttpClient client) => _client = client;

    public async Task SendAccountCreationConfirmationAsync(AccountCreationConfirmation confirmation)
    {
        await _client.PostAsJsonAsync("/accountCreationConfirmation", confirmation);
    }
}

public record AccountCreationConfirmation(string Email, string Name);