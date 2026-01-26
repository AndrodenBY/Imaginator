using Imaginator.Enums;
using Imaginator.Interfaces;
using Imaginator.Renderers;

namespace Imaginator.Factories;

public static class RendererFactory
{
    public static IAsciiRenderer RenderAscii(RenderMode mode)
    {
        return mode switch
        {
            RenderMode.Colored => new ColoredAsciiRenderer(),
            _ => new PlainAsciiRenderer()
        };
    }
}
