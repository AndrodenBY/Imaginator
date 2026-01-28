using Imaginator.Constants;
using Imaginator.Enums;
using Imaginator.Factories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Graphics;

public static class AsciiShow
{
    private static readonly LoaderFactory Factory = new();
    
    public static async Task Imaginate(string source, RenderMode mode)
    {
        try
        {
            var imageLoader = Factory.GetLoader(source);
            
            await using var imageStream = await imageLoader.GetImageStream(source);
            using var loadedImage = await Image.LoadAsync<Rgba32>(imageStream);

            if (loadedImage.Frames.Count > 1)
            {
                await TerminalCinema.ShowAnimatedImageInAscii(loadedImage, mode);
            }
            else
            {
                TerminalCinema.ShowStaticImageInAscii(loadedImage, mode);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine($"{ImaginatorMetadata.ErrorMessage}: {exception.Message}");
        }
    }
}
