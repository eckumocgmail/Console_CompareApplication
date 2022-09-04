using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/// <summary>
///   Выполняет сравнение директоии по составу файлов, 
/// с учетом всех вложенных подкаталогов.
/// 1) Получает список всех файлов для каждой директории
/// 2) Сравнивает по составу, результат сравнения: 
/// Список отсутсвующих файлов для каждого из файлов
/// </summary>
public struct CompareDirectories
{
    public string dir1 { get; set; }
    public string dir2 { get; set; }

    /// <summary>
    /// Уникальные файлы
    /// </summary>
    public IEnumerable<string> Outer { get; private set; }

    /// <summary>
    /// Фантомные копии файлов
    /// </summary>
    public IEnumerable<string> Inner { get; private set; }

    /// <summary>
    /// Файлы каталога 1 не содержащиеся в каталоге 2
    /// </summary>
    public IEnumerable<string> DeltaLeft { get; private set; }

    /// <summary>
    /// Файлы каталога 1
    /// </summary>
    public HashSet<string> ContentsLeft { get; private set; }

    /// <summary>
    /// Файлы каталога 2
    /// </summary>
    public HashSet<string> ContentsRight { get; private set; }

    /// <summary>
    /// Файлы каталога 2 не содержащиеся в каталоге 1
    /// </summary>
    public IEnumerable<string> DeltaRight { get; private set; }
    public object Details { get; private set; }
    public IEnumerable<CompareFilesProgram.TextCompareResult> Changes { get; private set; }

    public CompareDirectories(string dir1, string dir2) : this()
    {
        this.dir1 = dir1;
        this.dir2 = dir2;
    }

    public override string ToString()
    {
        string text = "Результат сравнения каталогов: \n";
        text += "\t" + dir1 + "\n";
        text += "\t" + dir2 + "\n";
        text += "В обоих каталогах найдены файлы: \n";
        for(int i = 0; i < Inner.Count(); i++)
        {
            text += "\t" + Inner.ToList()[i] + "\n";
            text += "\t" + Changes.ToList()[i] + "\n";
        }
            
        text += "В одном каталогах найдены файлы: \n";
        foreach (string file in Outer)
            text += "\t" + file + "\n";
        return text+"\n\n";
    }


    /// <summary>
    /// Сравнение директорий
    /// </summary>
    public static CompareDirectories Compare( string dir1, string dir2 )
    {
        
        Func<string, HashSet<string>> GetFileNames = (dir) =>
        {
            return System.IO.Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories).Select(path => Path.GetFileName(path)).ToHashSet();                
        };
            
        var files1 = GetFileNames(dir1);
        var files2 = GetFileNames(dir2);
        var inner = files1.Intersect(files2);

        var outer = files1.Union<string>(files2).Except(inner);
        
       
        var dev1 = files1.Except(files2);
        var dev2 = files2.Except(files1);
        return new CompareDirectories(dir1, dir2)
        {
            ContentsLeft =      files1,
            ContentsRight =     files2,
            Outer = outer,
            Inner = inner,

            DeltaLeft = dev1,
            DeltaRight = dev2,
            Changes = inner.Select(
                file => CompareFilesProgram.Compare(
                    files1.First(f => f.EndsWith(file)), 
                    files2.First(f => f.EndsWith(file))
                )
            )

        };
    }
}
