using Imaginator.Rendering;

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
} //Записывать каждый фрейм в массив а потом выводить эти массивы попеременно
//Посмотреть что быстрее рендеринг или вывод на консоль
//Трехмерный массив возможно получится
//комманды для тулы CLI
//Деплой ее как отдельный package
//Кеширование ее или еще чет
//Span vs StringBuilder
