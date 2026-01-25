using Imaginator.Constants;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Imaginator.Helpers;

public static class MediaHelper
{
    public static int GetDelay(ImageFrame<Rgba32> frame)
    {
        var metadata = frame.Metadata.GetGifMetadata();
        return metadata.FrameDelay > 0 
            ? metadata.FrameDelay * MediaSettings.GifTickToMilliseconds 
            : MediaSettings.DefaultFrameDelay;
    }
}
