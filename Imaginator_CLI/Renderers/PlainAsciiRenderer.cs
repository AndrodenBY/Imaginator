using Imaginator.Helpers;
using Imaginator.Interfaces;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Renderers;

public class PlainAsciiRenderer: IAsciiRenderer
{
    public string RenderPixel(Rgba32 pixel)
    {
        return AnsiColorMapper.MapToAscii(pixel).ToString();
    }
}
