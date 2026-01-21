namespace Imaginator.Interfaces;

public interface IImageLoader
{
    Task<Stream> GetImageStream(string source);
}
