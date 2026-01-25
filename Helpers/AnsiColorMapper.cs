using Imaginator.Constants;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Helpers;

public static class AnsiColorMapper
{
    public static char MapToAscii(Rgba32 pixel)
    {
        var brightness = (pixel.R + pixel.G + pixel.B) / MediaSettings.Channels;
        var index = brightness * (RenderSettings.AsciiSymbols.Length - MediaSettings.IndexOffset) / RenderSettings.MaxByteValue;
        return RenderSettings.AsciiSymbols[index];
    }
}
