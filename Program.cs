while (true)
{
    Console.Write("Write filepath: ");
    string? path = Console.ReadLine();
    
    if (String.IsNullOrWhiteSpace(path))
    {
        continue;
    }
    
    Imaginator.AsciiMilator.Imaginate(path);
    Console.WriteLine("Press Control+C to exit...");
}
