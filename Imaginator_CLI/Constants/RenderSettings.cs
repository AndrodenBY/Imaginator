namespace Imaginator.Constants;

public static class RenderSettings
{
    private const int SymbolLength = 1;
    public const int MaxByteValue = 255;
    public const int MaxAsciiWidth = 100;
    public const double RedWeight = 0.299;
    public const int LineOverhead = 4 + 2;
    public const double BlueWeight = 0.114;
    public const double GreenWeight = 0.587;
    public const double FontAspectRatio = 2.1;
    public const int BufferServicePadding = 64;
    public const int PlainPixelStringLength = 1;
    public const int ColorPixelStringLength = 20;
    public const string AsciiSymbols = " .:-=+*#%@";
    private const int MaxAnsiColorCodeLength = 7 + 11 + 1; 
    public const int MaxCharsPerPixel = MaxAnsiColorCodeLength + SymbolLength + 4; 
}
//public const string AsciiSymbols = " .:-=+*#%@$&WM8";
//public const string AsciiSymbols = " .,:;i1tfLCOG08@#";
