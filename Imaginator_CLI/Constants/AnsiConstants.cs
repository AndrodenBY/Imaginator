namespace Imaginator.Constants;

public static class AnsiConstants
{
    private const string Escape = "\u001b[";
    public const string ResetCursor = Escape + "H";
    private const string ResetStyle = Escape + "0m";
    private const string ClearScreen = Escape + "2J";
    public const string HideCursor = Escape + "?25l";
    private const string ShowCursor = Escape + "?25h";
    public const string FrameLineEnd = Escape + "K\n";
    public const string ForegroundRgb = Escape + "38;2;";
    public const string ResetTerminal = ResetStyle + ShowCursor;
    public const string PrepareCanvas = ClearScreen + HideCursor + ResetCursor;
}   
