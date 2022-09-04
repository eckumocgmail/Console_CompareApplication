using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// Программа сравнивает содержимое каталогов
/// </summary>
internal class CompareDirectoriesProgram
{

    /// <summary>
    /// Системный каталог
    /// </summary>
    private const string SystemRootPath = @"D:\System-Config";

    /// <summary>
    /// Последняя зафиксированная точка
    /// </summary>
    private static long LastActive = 0;

    /// <summary>
    /// Выполнение программы
    /// </summary>
    /// <param name="args"></param>
    internal static void Run(string[] args)
    {
        if(args.Length == 0)
        {
            string path = ReadPath();
            FixTime();
            Console.WriteLine
            (
                CompareDirectories.Compare(
                    System.IO.Directory.GetCurrentDirectory(),
                    path
                )
            );
            Console.WriteLine($"Продолжительность: {FixTime()}");
        }
        else
        {
            FixTime();
            if (args.Length == 1)
            {
                Console.WriteLine
                (                   
                    CompareDirectories.Compare(
                        System.IO.Directory.GetCurrentDirectory(),
                        args[0]
                    ) 
                );
                Console.WriteLine($"Продолжительность: {FixTime()}");
            }
            else
            {
                var argsList = new List<string>(args);
                for(int i=0; i<args.Length; i++)
                {
                    for (int j = 0; j < args.Length; j++)
                    {
                        if (i == j)
                            continue;
                        FixTime();
                        Console.WriteLine($"{ args[i]}\\n{ args[j]}");
                        Console.WriteLine
                        (
                                    
                            CompareDirectories.Compare(
                                    args[i],
                                    args[j]
                                ).ToString()

                        ); ;
                        Console.WriteLine($"Продолжительность: {FixTime()}");
                    }
                }
            }
        }        
    }


    /// <summary>
    /// Ввод пути к каталогу для сравнения с текущим
    /// </summary>
    private static string ReadPath()
    {
        Console.WriteLine("Введите путь к каталогу который необходимо " +
            "сравнить с текущим \n\t"+System.IO.Directory.GetCurrentDirectory());
        Console.Write(">");
        return Console.ReadLine();
    }

    /// <summary>
    /// Фиксация текущего времени
    /// </summary>    
    private static long FixTime()
    {
        if (LastActive == 0)
        {
            LastActive = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return FixTime();
        }
        else
        {
            long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long result = time - LastActive;
            LastActive = time;
            return result;
        }
    }
}

