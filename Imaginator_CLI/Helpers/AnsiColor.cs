using Imaginator.Constants;

namespace Imaginator.Helpers;

public static class AnsiColor
{
    public static string MakeRgb(byte r, byte g, byte b)
    {
        return $"{AnsiConstants.ForegroundRgb}{r};{g};{b}m";
    }
}
