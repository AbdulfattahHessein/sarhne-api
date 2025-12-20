using Shared.Services.Interfaces;

namespace Shared.Services;

public class UriService(string baseUri) : IUriService
{
    public string BaseUrl { get; } = baseUri;
}
