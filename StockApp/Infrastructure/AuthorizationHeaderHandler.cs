namespace StockApp.Infrastructure;

public class AuthorizationHeaderHandler : DelegatingHandler
{
    private readonly IdTokenProvider _tokenProvider;

    public AuthorizationHeaderHandler(IdTokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetIdTokenAsync();
        if (token != null)
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
