namespace Imaginator.Helpers;

public static class RenderBuffer
{
    public static int AddStringToBuffer(string text, char[] buffer, int position)
    {
        text.AsSpan().CopyTo(buffer.AsSpan(position));
        return text.Length;
    }
}
