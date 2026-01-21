using System.Text;
using Imaginator.Enums;
using Imaginator.Factories;
using Imaginator.Helpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Imaginator.Graphics;

public static class AsciiMulator
{
    public static void RenderStaticImageInAscii(Image<Rgba32> loadedImage, RenderMode mode)
    {
        var renderWidth = 100;
        var renderHeight = (int)(loadedImage.Height * ((double)renderWidth / loadedImage.Width) / Constants.HeightAspectDivisor);
        
        loadedImage.Mutate(x => x.Resize(renderWidth, renderHeight));
        
        var frameBuilder = new StringBuilder(renderWidth * renderHeight);
        var renderer = RendererFactory.RenderAscii(mode);
        
        loadedImage.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                ReadOnlySpan<Rgba32> rowSpan = accessor.GetRowSpan(y);

                foreach (var pixel in rowSpan)
                {
                    frameBuilder.Append(renderer.RenderPixel(pixel));
                }

                frameBuilder.AppendLine();
            }
        });

        Console.Write(frameBuilder.ToString());
    }
}
