using Imaginator.Interfaces;
using Imaginator.Loaders;

namespace Imaginator.Factories;

public class LoaderFactory
{
    private readonly Lazy<LocalLoader> _localLoader = new(() => new LocalLoader());
    private readonly Lazy<WebLoader> _webLoader = new(() => new WebLoader());
    
    public IImageLoader GetLoader(string source)
    {
        var cleanSource = source.Trim().ToLower();

        if (cleanSource.StartsWith("http://") || cleanSource.StartsWith("https://"))
        {
            return _webLoader.Value;
        }

        return _localLoader.Value;
    }
}
