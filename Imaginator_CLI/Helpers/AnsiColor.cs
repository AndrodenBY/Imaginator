using System.Text;
using Imaginator.ColorPresets;
using Imaginator.Constants;

namespace Imaginator.Helpers;

public static class AnsiColor
{
    public static string MakeRgb(byte red, byte green, byte blue)
    {
        return $"{AnsiConstants.ForegroundRgb}{red};{green};{blue}m";
    }
    
    public static string MakeMonochrome(ColorPreset preset, byte brightness)
    {
        var factor = brightness / (double)RenderSettings.MaxByteValue;
        var red = (byte)(preset.R * factor);
        var green = (byte)(preset.G * factor);
        var blue = (byte)(preset.B * factor);
        
        return $"{AnsiConstants.ForegroundRgb}{red};{green};{blue}m";
    }
}
