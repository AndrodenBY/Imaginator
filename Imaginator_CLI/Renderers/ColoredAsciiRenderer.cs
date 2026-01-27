using System.Text;
using Imaginator.ColorPresets;
using Imaginator.Constants;
using Imaginator.Helpers;
using Imaginator.Interfaces;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Renderers;

public class ColoredAsciiRenderer : IAsciiRenderer
{
    public string RenderPixel(Rgba32 pixel)
    {
        var data = AnsiColorMapper.MapAscii(pixel);
        var color = AnsiColor.MakeRgb(pixel.R, pixel.G, pixel.B);
        
        return $"{color}{data.Symbol}";
    }
}
