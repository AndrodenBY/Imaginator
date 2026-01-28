using System.Text;
using Imaginator.Helpers;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Interfaces;

public interface IAsciiRenderer
{
    int WriteColor(Rgba32 pixel, AsciiData data, char[] target, int position);
    char GetSymbol(AsciiData data);
}
