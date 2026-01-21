using Imaginator.Interfaces;

namespace Imaginator.Loaders;

public class WebLoader: IImageLoader
{
    private static readonly HttpClient HttpClient = new();

    static WebLoader()
    {
        HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(Constants.UserAgentHeader);
    }
    
    public async Task<Stream> GetImageStream(string source)
    {
        var response = await HttpClient.GetAsync(source, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStreamAsync();
    }
}
