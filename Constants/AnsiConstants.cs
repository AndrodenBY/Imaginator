namespace Imaginator.Constants;

public static class AnsiConstants
{
    private const string Escape = "\u001b[";
    public const string FrameLineEnd = Escape + "K\n";
    public const string ResetCursor = Escape + "H";
    public const string ClearScreen = Escape + "2J";
    public const string HideCursor = Escape + "?25l";
    public const string ShowCursor = Escape + "?25h";
    public const string ResetStyle = Escape + "0m";
    public const string PrepareCanvas = ClearScreen + HideCursor + ResetCursor;
    public const string ResetTerminal = ResetStyle + ShowCursor;
}   
