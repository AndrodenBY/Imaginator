using Imaginator.Constants;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Helpers;

public static class TimeHelper
{
    public static int GetDelay(ImageFrame<Rgba32> frame)
    {
        var metadata = frame.Metadata.GetGifMetadata();
        return metadata.FrameDelay > 0 
            ? metadata.FrameDelay * MediaSettings.GifTickToMilliseconds 
            : MediaSettings.DefaultFrameDelay;
    }
    
    public static bool IsExitRequested()
    {
        if (!Console.KeyAvailable) 
            return false;

        var key = Console.ReadKey(intercept: true).Key;
        return key is ConsoleKey.Escape or ConsoleKey.Enter;
    }
}
