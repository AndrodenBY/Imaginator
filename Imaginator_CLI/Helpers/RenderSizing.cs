using Imaginator.Constants;
using Imaginator.Enums;

namespace Imaginator.Helpers;

public static class RenderSizing
{
    public static (int Width, int Height) GetRenderSize(int originalWidth, int originalHeight)
    {
        var windowWidth = Console.WindowWidth > 0 
            ? Console.WindowWidth - TerminalConstants.Padding 
            : TerminalConstants.DefaultWidth;
        
        var renderWidth = Math.Min(windowWidth, RenderSettings.MaxAsciiWidth);
        
        var renderHeight = (int)(originalHeight * (double)renderWidth / originalWidth / RenderSettings.FontAspectRatio);
        
        var windowHeight = Console.WindowHeight > 0 
            ? Console.WindowHeight - TerminalConstants.Padding 
            : TerminalConstants.DefaultHeight;

        if (renderHeight > windowHeight) renderHeight = windowHeight;
        
        return (Math.Max(1, renderWidth), Math.Max(1, renderHeight));
    }
    
    public static int GetBufferCapacity(int width, int height, RenderMode mode)
    {
        var pixelDensity = mode == RenderMode.Colored 
            ? RenderSettings.ColorPixelStringLength 
            : RenderSettings.PlainPixelStringLength;

        var contentSize = width * height * pixelDensity;
        var lineWrappingSize = height * AnsiConstants.FrameLineEnd.Length;
        
        return contentSize + lineWrappingSize + RenderSettings.BufferServicePadding;
    }
}
