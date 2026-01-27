using System.Diagnostics;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Imaginator.Constants;
using Imaginator.Enums;
using Imaginator.Factories;
using Imaginator.Helpers;
using Imaginator.Interfaces;

namespace Imaginator.Graphics;

public static class AsciiMator
{
    public static void RenderStaticImageInAscii(Image<Rgba32> image, RenderMode mode)
    {
        Console.Write(AnsiConstants.PrepareCanvas);
    
        var renderer = RendererFactory.RenderAscii(mode);
        var frame = BuildFrame(image, renderer, mode);
    
        Console.Write(frame);
        Console.Write(AnsiConstants.ResetTerminal + "\n");
    }
    
    public static async Task RenderAnimatedImageInAscii(Image<Rgba32> gif, RenderMode mode)
    {
        Console.Clear();
        Console.Write(AnsiConstants.HideCursor);
        
        var renderer = RendererFactory.RenderAscii(mode);
        var isRunning = true;
        var stopwatch = new Stopwatch();

        while (isRunning)
        {
            for (int i = 0; i < gif.Frames.Count; i++)
            {
                stopwatch.Restart();
                
                using (var frameImage = gif.Frames.CloneFrame(i))
                {
                    var frameContent = BuildFrame(frameImage, renderer, mode);
                
                    Console.SetCursorPosition(0, 0);
                    Console.Write(AnsiConstants.ResetCursor + frameContent);
                }
                
                var gifDelay = TimeHelper.GetDelay(gif.Frames[i]);
                var remainingDelay = (int)stopwatch.ElapsedMilliseconds - gifDelay;
                
                if (remainingDelay > 0)
                {
                    await Task.Delay(remainingDelay);
                }

                if (TimeHelper.IsExitRequested())
                {
                    isRunning = false;
                    break;
                }
            }
        }

        Console.Write(AnsiConstants.ResetTerminal);
    }

    private static string BuildFrame(Image<Rgba32> loadedFrame, IAsciiRenderer renderer, RenderMode mode)
    {
        var (width, height) = RenderSizing.GetRenderSize(loadedFrame.Width, loadedFrame.Height);
        
        loadedFrame.Mutate(x => x.Resize(width, height));
        
        var capacity = RenderSizing.GetBufferCapacity(width, height, mode);
        var frameBuilder = new StringBuilder(capacity);

        loadedFrame.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                var rowSpan = accessor.GetRowSpan(y);
                foreach (ref readonly var pixel in rowSpan)
                {
                    frameBuilder.Append(renderer.RenderPixel(pixel));
                }
                frameBuilder.Append(AnsiConstants.ResetColor).Append(AnsiConstants.FrameLineEnd);
            }
        });

        return frameBuilder.ToString();
    }
}

