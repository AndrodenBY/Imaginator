using Imaginator.Enums;
using Imaginator.Factories;
using Imaginator.Interfaces;
using Imaginator.Loaders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Graphics;

public static class AsciiMilator
{
    private static readonly LoaderFactory Factory = new(new LocalLoader(), new WebLoader());
    
    public static async Task Imaginate(string source, RenderMode mode)
    {
        try
        {
            IImageLoader imageLoader = Factory.GetLoader(source);
            
            await using var imageStream = await imageLoader.GetImageStream(source);
            using var loadedImage = await Image.LoadAsync<Rgba32>(imageStream);
            AsciiMulator.RenderStaticImageInAscii(loadedImage, mode);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"{Constants.ErrorMessage}: {exception.Message}");
        }
    }
}
