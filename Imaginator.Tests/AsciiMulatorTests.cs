using FluentAssertions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Imaginator.Constants;
using Imaginator.Enums;
using Imaginator.Graphics;

namespace Imaginator.Tests;

[TestFixture]
public class AsciiMatorStaticTests
{
    [Test]
    public void RenderStaticImageInAscii_ShouldWriteToConsole_WhenImageIsValid()
    {
        // Arrange
        using var image = new Image<Rgba32>(2, 2);
        
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act
        AsciiMator.RenderStaticImageInAscii(image, RenderMode.Plain);

        // Assert
        var output = stringWriter.ToString();
        output.Should().NotBeNullOrEmpty();
        output.Should().ContainAny(RenderSettings.AsciiSymbols.Select(c => c.ToString()));
        output.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length.Should().BeGreaterThanOrEqualTo(2);
    }
    
    [Test]
    public void RenderStatic_ShouldNotContainAnsi_WhenModeIsPlain()
    {
        // Arrange
        var image = new Image<Rgba32>(1, 1);
    
        using var sw = new StringWriter();
        Console.SetOut(sw);

        // Act
        AsciiMator.RenderStaticImageInAscii(image, RenderMode.Plain);

        // Assert
        var result = sw.ToString();
        result.Should().NotContain("\u001b[", "because plain mode should only contain raw text");
    }
    
    
}
