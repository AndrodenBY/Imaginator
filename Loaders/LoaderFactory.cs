using Imaginator.Interfaces;

namespace Imaginator.Loaders;

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
