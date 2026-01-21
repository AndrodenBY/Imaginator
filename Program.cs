using Imaginator.Enums;
using Imaginator.Graphics;

while (true)
{
    Console.Write("Write filepath: ");
    var path = Console.ReadLine();
    Console.Write("Write mode(1 - plain, 2 - colored): ");
    var inputMode = Convert.ToInt32(Console.ReadLine()); 
    var mode = (RenderMode)inputMode;
    
    if (String.IsNullOrWhiteSpace(path))
    {
        continue;
    }
    
    await AsciiMilator.Imaginate(path, mode);
    Console.WriteLine("Press Control+C to exit...");
} 
