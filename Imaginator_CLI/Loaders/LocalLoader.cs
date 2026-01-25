using Imaginator.Helpers;
using Imaginator.Interfaces;

namespace Imaginator.Loaders;

public class LocalLoader: IImageLoader
{
    public Task<Stream> GetImageStream(string source)
    {
        return Task.FromResult<Stream>(FileStreamHelper.OpenRead(source));
    }
}
