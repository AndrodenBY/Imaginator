using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Runtime.InteropServices;
using Imaginator.Enums;
using OpenCvSharp;
using Imaginator.Factories;
using OpenCvSharp.CPlusPlus;

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
    
    private static readonly char[] CharSet = " .,:;i1tfLCOG08@#".ToCharArray();

    public static async Task RenderAnimatedImage(string filePath, RenderMode mode)
{
    using Image<Rgba32> gif = Image.Load<Rgba32>(filePath);
    
    Console.Clear();
    Console.Write("\u001b[?25l");

    while (true) 
    {
        for (int i = 0; i < gif.Frames.Count; i++)
        {
            var frame = gif.Frames[i];
            
            Console.SetCursorPosition(0, 0);
            Console.Write("\u001b[H");
            
            int windowWidth = Console.WindowWidth > 0 ? Console.WindowWidth - 1 : 80;
            int width = Math.Min(windowWidth, 100); 
            
            int height = (int)(frame.Height * width / frame.Width / 2.1);
            
            int windowHeight = Console.WindowHeight > 0 ? Console.WindowHeight - 1 : 40;
            if (height > windowHeight) height = windowHeight;

            if (height < 1) height = 1;
            
            using (var frameImage = gif.Frames.CloneFrame(i))
            {
                frameImage.Mutate(x => x.Resize(width, height));

                var sb = new StringBuilder();

                for (int y = 0; y < frameImage.Height; y++)
                {
                    for (int x = 0; x < frameImage.Width; x++)
                    {
                        var pixel = frameImage[x, y];
                        
                        int brightness = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                        char c = CharSet[brightness * (CharSet.Length - 1) / 255];

                        if (mode == RenderMode.Colored)
                        {
                            sb.AppendFormat("\u001b[38;2;{0};{1};{2}m{3}", pixel.R, pixel.G, pixel.B, c);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                    sb.Append("\u001b[K\n");
                }

                Console.Write(sb.ToString());
            }
            
            var gifMeta = frame.Metadata.GetGifMetadata();
            int delay = gifMeta.FrameDelay;
            await Task.Delay(delay > 0 ? delay * 10 : 100);
            
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                Console.Write("\u001b[0m\u001b[?25h");
                Console.Clear();
                return;
            }
        }
    }
}
}
