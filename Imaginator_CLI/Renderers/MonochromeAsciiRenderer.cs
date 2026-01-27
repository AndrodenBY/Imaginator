using System.Text;
using Imaginator.ColorPresets;
using Imaginator.Helpers;
using Imaginator.Interfaces;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Renderers;

public class MonochromeAsciiRenderer(ColorPreset preset) : IAsciiRenderer
{
    public string RenderPixel(Rgba32 pixel)
    {
        var data = AnsiColorMapper.MapAscii(pixel);
        var shade = AnsiColor.MakeMonochrome(preset, data.Brightness);
        
        return $"{shade}{data.Symbol}";
    }
}
