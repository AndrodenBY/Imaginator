using System.Diagnostics;
using Imaginator.Constants;
using Imaginator.Enums;
using Imaginator.Factories;
using Imaginator.Helpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Graphics;

public static class TerminalCinema
{
    public static void ShowStaticImageInAscii(Image<Rgba32> loadedImage, RenderMode mode)
    {
        Console.Write(AnsiConstants.PrepareCanvas);
    
        var renderer = RendererFactory.RenderAscii(mode);
        var frame = AsciiMator.BuildFrame(loadedImage, renderer);
    
        Console.Write(frame);
        Console.Write(AnsiConstants.ResetTerminal + "\n");
    }
    
    public static async Task ShowAnimatedImageInAscii(Image<Rgba32> loadedImage, RenderMode mode)
    {
        var cachedFrames = GetPreRenderFrames(loadedImage, mode);
        await AnimateInAscii(cachedFrames);
    }
    
    private static List<(string Content, int Delay)> GetPreRenderFrames(Image<Rgba32> loadedImage, RenderMode mode)
    {
        var renderer = RendererFactory.RenderAscii(mode);
        var cache = new List<(string Content, int Delay)>(loadedImage.Frames.Count);

        for (int i = 0; i < loadedImage.Frames.Count; i++)
        {
            using var frameImage = loadedImage.Frames.CloneFrame(i);
            var content = AsciiMator.BuildFrame(frameImage, renderer);
            var delay = TimeHelper.GetDelay(loadedImage.Frames[i]);

            cache.Add((content, delay));
        }

        return cache;
    }
    
    private static async Task AnimateInAscii(List<(string Content, int Delay)> frames)
    {
        Console.Clear();
        Console.Write(AnsiConstants.HideCursor);

        var stopwatch = new Stopwatch();
        var isRunning = true;

        while (isRunning)
        {
            foreach (var (content, delay) in frames)
            {
                stopwatch.Restart();

                Console.SetCursorPosition(0, 0);
                Console.Write(content);

                if (TimeHelper.IsExitRequested())
                {
                    isRunning = false;
                    break;
                }

                var remaining = delay - (int)stopwatch.ElapsedMilliseconds;
                if (remaining > 0)
                {
                    await Task.Delay(remaining);
                }
            }
        }

        Console.Write(AnsiConstants.ResetTerminal + AnsiConstants.ShowCursor);
    }
}
