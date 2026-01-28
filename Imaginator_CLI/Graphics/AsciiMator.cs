using System.Buffers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Imaginator.Constants;
using Imaginator.Helpers;
using Imaginator.Interfaces;

namespace Imaginator.Graphics;

public static class AsciiMator
{
    public static string BuildFrame(Image<Rgba32> frame, IAsciiRenderer renderer)
    {
        var (width, height) = RenderSizing.GetRenderSize(frame.Width, frame.Height);
        frame.Mutate(x => x.Resize(width, height));
        
        var pixelPayloadSize = width * height * RenderSettings.MaxCharsPerPixel;
        var lineFormattingSize = height * RenderSettings.LineOverhead;
        
        var bufferSize = pixelPayloadSize + lineFormattingSize;
        char[] buffer = ArrayPool<char>.Shared.Rent(bufferSize);

        try
        {
            var contentLength = RenderImageToBuffer(frame, renderer, buffer);
            return new string(buffer, 0, contentLength);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }
    
    private static int RenderImageToBuffer(Image<Rgba32> frame, IAsciiRenderer renderer, char[] buffer)
    {
        var position = 0;
        Rgba32 lastColor = default;

        frame.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgba32> rowSpan = accessor.GetRowSpan(y);
                
                position = RenderRow(rowSpan, renderer, buffer, position, ref lastColor);
                position += RenderBuffer.AddStringToBuffer(AnsiConstants.ResetColor, buffer, position);
                position += RenderBuffer.AddStringToBuffer(AnsiConstants.FrameLineEnd, buffer, position);
            }
        });

        return position;
    }
    
    private static int RenderRow(Span<Rgba32> row, IAsciiRenderer renderer, char[] buffer, int position, ref Rgba32 lastColor)
    {
        foreach (ref readonly var pixel in row)
        {
            var asciiData = AnsiColorMapper.MapAscii(pixel);
            
                position += renderer.WriteColor(pixel, asciiData, buffer, position);
                lastColor = pixel;
            
            buffer[position++] = renderer.GetSymbol(asciiData);
        }

        return position;
    }
}

