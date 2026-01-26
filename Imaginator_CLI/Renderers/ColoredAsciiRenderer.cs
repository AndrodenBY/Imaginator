using Imaginator.Constants;
using Imaginator.Helpers;
using Imaginator.Interfaces;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Renderers;

public class ColoredAsciiRenderer : IAsciiRenderer
{
    public string RenderPixel(Rgba32 pixel)
    {
        var ascii = AnsiColorMapper.MapToAscii(pixel); 
        var color = AnsiColor.MakeRgb(pixel.R, pixel.G, pixel.B); 
        
        return $"{color}{ascii}";
    }
}
