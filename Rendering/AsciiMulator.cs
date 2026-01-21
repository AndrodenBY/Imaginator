using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Imaginator.Rendering;

public static class AsciiMulator
{
    public static void RenderStaticImageInAscii(Image<Rgba32> loadedImage)
    {
        var renderWidth = 100;
        var renderHeight = (int)(loadedImage.Height * ((double)renderWidth / loadedImage.Width) / Constants.HeightAspectDivisor);
        
        loadedImage.Mutate(x => x.Resize(renderWidth, renderHeight));
        
        var frameBuilder = new StringBuilder(renderWidth * renderHeight);
        
        loadedImage.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                ReadOnlySpan<Rgba32> rowSpan = accessor.GetRowSpan(y);

                foreach (var pixel in rowSpan)
                {
                    var brightness = (pixel.R + pixel.G + pixel.B) / Constants.Channels;
                    var index = brightness * (Constants.AsciiSymbols.Length - Constants.IndexOffset) / Constants.MaxByteValue;

                    frameBuilder.Append(Constants.AsciiSymbols[index]);
                }
                frameBuilder.AppendLine();
            }
        });

        Console.Write(frameBuilder.ToString());
    }
}
