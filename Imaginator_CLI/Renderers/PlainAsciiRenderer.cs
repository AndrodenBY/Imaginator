
using Imaginator.Helpers;
using Imaginator.Interfaces;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Renderers;

public class PlainAsciiRenderer : IAsciiRenderer
{
    public int WriteColor(Rgba32 pixel, AsciiData data, char[] target, int position) => 0;

    public char GetSymbol(AsciiData data) => data.Symbol;
}
