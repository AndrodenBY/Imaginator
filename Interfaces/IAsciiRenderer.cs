using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Interfaces;

public interface IAsciiRenderer
{
    string RenderPixel(Rgba32 pixel);
}
