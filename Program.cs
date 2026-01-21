using Imaginator.Render;

while (true)
{
    Console.Write("Write filepath: ");
    string? path = Console.ReadLine();
    
    if (String.IsNullOrWhiteSpace(path))
    {
        continue;
    }
    
    await AsciiMilator.Imaginate(path);
    Console.WriteLine("Press Control+C to exit...");
}
