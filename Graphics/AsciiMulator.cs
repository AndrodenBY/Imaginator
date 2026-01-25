using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Imaginator.Constants;
using Imaginator.Enums;
using Imaginator.Factories;
using Imaginator.Helpers;

namespace Imaginator.Graphics;

public static class AsciiMulator
{
    private static bool _isRunning = true;
    
    public static void RenderStaticImageInAscii(Image<Rgba32> loadedImage, RenderMode mode)
    {
        var (width, height) = RenderSizing.GetRenderSize(loadedImage.Width, loadedImage.Height);
    
        loadedImage.Mutate(x => x.Resize(width, height));
        
        var capacity = RenderSizing.GetBufferCapacity(width, height, mode);
        var frameBuilder = new StringBuilder(capacity);
        
        frameBuilder.Append(AnsiConstants.HideCursor);
        frameBuilder.Append(AnsiConstants.ClearScreen);
        frameBuilder.Append(AnsiConstants.ToHome);

        var renderer = RendererFactory.RenderAscii(mode);
    
        loadedImage.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                ReadOnlySpan<Rgba32> rowSpan = accessor.GetRowSpan(y);

                foreach (ref readonly var pixel in rowSpan)
                {
                    frameBuilder.Append(renderer.RenderPixel(pixel));
                }
            
                frameBuilder.Append(AnsiConstants.FrameLineEnd);
            }
        });
        frameBuilder.Append(AnsiConstants.ResetTerminal);
        Console.Write(frameBuilder.ToString());
    }
    
    public static async Task RenderAnimatedImage(Image<Rgba32> loadedGif, RenderMode mode)
    {
        Console.Clear();
        Console.Write(AnsiConstants.HideCursor);
        
        while (_isRunning)
        {
            for (int i = 0; i < loadedGif.Frames.Count; i++)
            {
                var gifFrame = loadedGif.Frames[i];
                
                Console.SetCursorPosition(0, 0);
                Console.Write(AnsiConstants.ResetCursor);
                
                var (width, height) = RenderSizing.GetRenderSize(gifFrame.Width, gifFrame.Height);
                
                using (var frameImage = loadedGif.Frames.CloneFrame(i))
                {
                    frameImage.Mutate(x => x.Resize(width, height));

                    var stringBuilder = new StringBuilder();

                    for (int y = 0; y < frameImage.Height; y++)
                    {
                        for (int x = 0; x < frameImage.Width; x++)
                        {
                            var pixel = frameImage[x, y];
                            var character = GetAsciiCharacter(pixel);

                            if (mode == RenderMode.Colored)
                            {
                                stringBuilder.AppendFormat("{Escape}38;2;{0};{1};{2}m{3}", AnsiConstants.Escape, pixel.R, pixel.G, pixel.B, character);
                            }
                            else
                            {
                                stringBuilder.Append(character);
                            }
                        }
                        stringBuilder.Append(AnsiConstants.FrameLineEnd);
                    }

                    Console.Write(stringBuilder.ToString());
                }
                
                await Task.Delay(MediaHelper.GetDelay(gifFrame));
                
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(false);
                    _isRunning = false;
                    break; 
                }
            }
        }
        
        Console.Write(AnsiConstants.ResetTerminal);
        Console.Clear();
    }
    
    private static char GetAsciiCharacter(Rgba32 pixel)
    {
        var brightness = pixel.R * RenderSettings.RedWeight + pixel.G * RenderSettings.GreenWeight + pixel.B * RenderSettings.BlueWeight;
        var index = (int)(brightness * (RenderSettings.AsciiSymbols.Length - MediaSettings.IndexOffset) / RenderSettings.MaxByteValue);

        return RenderSettings.AsciiSymbols[index];
    }
}

