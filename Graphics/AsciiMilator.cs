using Imaginator.Constants;
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
            var imageLoader = Factory.GetLoader(source);
            
            await using var imageStream = await imageLoader.GetImageStream(source);
            using var loadedImage = await Image.LoadAsync<Rgba32>(imageStream);

            if (loadedImage.Frames.Count > 1)
            {
                await AsciiMulator.RenderAnimatedImageInAscii(loadedImage, mode);
            }
            else
            {
                AsciiMulator.RenderStaticImageInAscii(loadedImage, mode);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine($"{ProgramConstants.ErrorMessage}: {exception.Message}");
        }
    }
}
