using Imaginator.ColorPresets;
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
            RenderMode.RedOnly => new MonochromeAsciiRenderer(MonochromePresets.RedOnly),
            RenderMode.GreenOnly => new MonochromeAsciiRenderer(MonochromePresets.FalloutGreen),
            RenderMode.BlueOnly => new MonochromeAsciiRenderer(MonochromePresets.BlueOnly),
            _ => new PlainAsciiRenderer()
        };
    }
}
