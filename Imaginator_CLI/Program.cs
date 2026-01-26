using Imaginator.Enums;
using Imaginator.Graphics;

while (true)
{
    Console.Write("Write filepath or URL: ");
    var path = Console.ReadLine();
    
    if (!File.Exists(path))
    {
        Console.WriteLine("File not found.");
        continue;
    }
    
    Console.Write("Plain(1) or Colored(2): ");
    var input = Console.ReadLine();
    
    if (!Enum.TryParse<RenderMode>(input, out var mode))
    {
        mode = RenderMode.Plain;
    }
    
    if (string.IsNullOrWhiteSpace(path)) continue;

    await AsciiShow.Imaginate(path, mode);
}
