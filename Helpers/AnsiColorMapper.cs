using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Helpers;

public static class AnsiColorMapper
{
    public static char MapToAscii(Rgba32 pixel)
    {
        var brightness = (pixel.R + pixel.G + pixel.B) / Constants.Channels;
        var index = brightness * (Constants.AsciiSymbols.Length - Constants.IndexOffset) / Constants.MaxByteValue;
        return Constants.AsciiSymbols[index];
    }
}
