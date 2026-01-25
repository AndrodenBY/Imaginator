using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Imaginator.Constants;
using Imaginator.Enums;
using Imaginator.Factories;
using Imaginator.Helpers;
using Imaginator.Interfaces;
using Imaginator.Renderers;

namespace Imaginator.Graphics;

public static class AsciiMulator
{
    
    public static void RenderStaticImageInAscii(Image<Rgba32> image, RenderMode mode)
    {
        Console.Write(AnsiConstants.PrepareCanvas);
    
        var renderer = RendererFactory.RenderAscii(mode);
        var frame = BuildFrame(image, renderer, mode);
    
        Console.Write(frame);
        Console.Write(AnsiConstants.ResetTerminal + "\n");
    }
    
    public static async Task RenderAnimatedImage(Image<Rgba32> gif, RenderMode mode)
    {
        Console.Clear();
        Console.Write(AnsiConstants.HideCursor);
        
        IAsciiRenderer renderer = mode == RenderMode.Colored 
            ? new ColoredAsciiRenderer() 
            : new PlainAsciiRenderer();

        bool isRunning = true;

        while (isRunning)
        {
            for (int i = 0; i < gif.Frames.Count; i++)
            {
                using (var frameImage = gif.Frames.CloneFrame(i))
                {
                    string frameContent = BuildFrame(frameImage, renderer, mode);
                
                    Console.SetCursorPosition(0, 0);
                    Console.Write(AnsiConstants.ResetCursor + frameContent);
                }
                
                var delay = gif.Frames[i].Metadata.GetGifMetadata().FrameDelay;
                await Task.Delay(delay > 0 ? delay * 10 : 100);

                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    isRunning = false;
                    break;
                }
            }
        }

        Console.Write(AnsiConstants.ResetTerminal);
    }

    private static string BuildFrame(Image<Rgba32> frame, IAsciiRenderer renderer, RenderMode mode)
    {
        var (width, height) = RenderSizing.GetRenderSize(frame.Width, frame.Height);
        
        frame.Mutate(x => x.Resize(width, height));
        
        var capacity = RenderSizing.GetBufferCapacity(width, height, mode);
        var sb = new StringBuilder(capacity);

        frame.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                var rowSpan = accessor.GetRowSpan(y);
                foreach (ref readonly var pixel in rowSpan)
                {
                    sb.Append(renderer.RenderPixel(pixel));
                }
                sb.Append(AnsiConstants.FrameLineEnd);
            }
        });

        return sb.ToString();
    }
    
    private static char GetAsciiCharacter(Rgba32 pixel)
    {
        var brightness = pixel.R * RenderSettings.RedWeight + pixel.G * RenderSettings.GreenWeight + pixel.B * RenderSettings.BlueWeight;
        var index = (int)(brightness * (RenderSettings.AsciiSymbols.Length - MediaSettings.IndexOffset) / RenderSettings.MaxByteValue);

        return RenderSettings.AsciiSymbols[index];
    }
}

