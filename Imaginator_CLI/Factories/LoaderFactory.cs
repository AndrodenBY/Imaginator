using Imaginator.Interfaces;
using Imaginator.Loaders;

namespace Imaginator.Factories;

public class LoaderFactory(LocalLoader localLoader, WebLoader webLoader)
{
    public IImageLoader GetLoader(string source)
    {
        string cleanSource = source.Trim().ToLower();

        if (cleanSource.StartsWith("http://") || cleanSource.StartsWith("https://"))
        {
            return webLoader;
        }

        return localLoader;
    }
}
