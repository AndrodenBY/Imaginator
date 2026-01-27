using Imaginator.Constants;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Helpers;

public static class AnsiColorMapper
{
    public static AsciiData MapAscii(Rgba32 pixel)
    {
        var brightness = pixel.R * RenderSettings.RedWeight + pixel.G * RenderSettings.GreenWeight + pixel.B * RenderSettings.BlueWeight;
    
        var maxIndex = RenderSettings.AsciiSymbols.Length - MediaSettings.IndexOffset;
        var index = (int)(brightness * maxIndex / RenderSettings.MaxByteValue);
        index = Math.Clamp(index, 0, maxIndex);
    
        return new AsciiData(RenderSettings.AsciiSymbols[index], (byte)Math.Clamp(brightness, 0, RenderSettings.MaxByteValue));
    }
}
