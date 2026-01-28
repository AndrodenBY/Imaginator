using System.Text;
using Imaginator.ColorPresets;
using Imaginator.Constants;
using Imaginator.Helpers;
using Imaginator.Interfaces;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Renderers;

public class ColoredAsciiRenderer : IAsciiRenderer
{
    public int WriteColor(Rgba32 pixel, AsciiData data, char[] target, int position)
    {
        var start = position;
        
        // Write the prefix: \u001b[38;2;
        AnsiConstants.ForegroundRgb.AsSpan().CopyTo(target.AsSpan(position));
        position += AnsiConstants.ForegroundRgb.Length;
        
        position += WriteByteAsText(pixel.R, target, position);
        target[position++] = ';';
        position += WriteByteAsText(pixel.G, target, position);
        target[position++] = ';';
        position += WriteByteAsText(pixel.B, target, position);
        target[position++] = 'm';

        return position - start;
    }
    
    public char GetSymbol(AsciiData data) => data.Symbol;

    private static int WriteByteAsText(byte value, char[] target, int pos)
    {
        if (value == 0) { target[pos] = '0'; return 1; }
        var count = value >= 100 ? 3 : value >= 10 ? 2 : 1;
        for (var i = count - 1; i >= 0; i--)
        {
            target[pos + i] = (char)('0' + (value % 10));
            value /= 10;
        }
        return count;
    }
}
