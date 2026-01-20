using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator;

public static class AsciiMilator
{
    public static void Imaginate(string path)
    {
        try
        {
            using var loadedImage = Image.Load<Rgba32>(path);
            RenderInAscii(loadedImage);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"{Constants.ErrorMessage}: {exception.Message}");
        }
    }

    private static void RenderInAscii(Image<Rgba32> loadedImage)
    {
        var renderWidth = 100;
        var renderHeight = (int)(loadedImage.Height * ((double)renderWidth / loadedImage.Width) / Constants.HeightAspectDivisor);

        for (int y = 0; y < renderHeight; y++)
        {
            for (int x = 0; x < renderWidth; x++)
            {
                var originalX = x * loadedImage.Width / renderWidth;
                var originalY = y * loadedImage.Height / renderHeight;

                var pixel = loadedImage[originalX, originalY];
                var brightness = (pixel.R + pixel.G + pixel.B) / Constants.Channels;
                var index = brightness * (Constants.AsciiSymbols.Length - Constants.IndexOffset) / Constants.MaxByteValue;

                Console.Write(Constants.AsciiSymbols[index]);
            }
            Console.WriteLine();
        }
    }
}
