using Imaginator.ColorPresets;
using Imaginator.Helpers;
using Imaginator.Interfaces;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Renderers;

public class MonochromeAsciiRenderer : IAsciiRenderer
{
    private readonly string[] _shadeCache;

    public MonochromeAsciiRenderer(ColorPreset preset)
    {
        _shadeCache = new string[256];
        
        for (int i = 0; i < 256; i++)
        {
            _shadeCache[i] = AnsiColor.MakeMonochrome(preset, (byte)i);
        }
    }

    public int WriteColor(Rgba32 pixel, AsciiData data, char[] target, int pos)
    {
        var ansiCode = _shadeCache[data.Brightness];
        ansiCode.AsSpan().CopyTo(target.AsSpan(pos));
        
        return ansiCode.Length;
    }

    public char GetSymbol(AsciiData data) => data.Symbol;
}
