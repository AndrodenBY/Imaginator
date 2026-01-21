namespace Imaginator.Helpers;

public static class AnsiColor
{
    private const string Escape = "\u001b[";
    private const string ForegroundRgb = "38;2;";

    public static string Rgb(byte r, byte g, byte b)
    {
        return $"{Escape}{ForegroundRgb}{r};{g};{b}m";
    }
    
    public const string Reset = "\u001b[0m";
}
