using Console_CompareApplication.CompareModule;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_CompareApplication
{
    using static InputApplicationProgram;
    internal class CompareProgram 
    {
        internal static void RunAtConsole(ref string[] args)
        {                      
            string dir1 = InputApplicationProgram.SelectDirectory(ref args);
            string dir2 = InputApplicationProgram.SelectDirectory(ref args);
            CompareDirectories(dir1,dir2);
        }

        internal static UpdateDirectory CompareDirectories(string dir1, string dir2)
        {
            Func<string, HashSet<dynamic>> GetFiles = (dir) =>
            {
                return System.IO.Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
                    .Select(path => new
                    {
                        Name = path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar)+1),
                        Path = path,
                        Uri = path.Substring(dir.Length)
                    }).ToHashSet<dynamic>();
            };

            var files1 = GetFiles(dir1);
            var files2 = GetFiles(dir2);

            var deleted = files2.Select(f => (string)f.Uri).Where(uri => files1.Select(f1 => f1.Uri).Contains((string)uri)==false ).ToList();
            var created = files1.Select(f => (string)f.Uri).Where(uri => files1.Select(f2 => f2.Uri).Contains((string)uri)==false ).ToList();
            var updated = files1.Select(f => (string)f.Uri).Where(uri => files1.Select(f2 => f2.Uri).Contains((string)uri)==true ).ToList();
            var creates = new Dictionary<string, string>(
                created.Select( file => new KeyValuePair<string, string>( file, System.IO.File.ReadAllText(Path.Combine(dir2,file)) ))
            );
            var updates = new Dictionary<string, UpdateFile>(updated.Select(file => new KeyValuePair<string, UpdateFile>(file, CompareFiles.CompareTextFiles(Path.Combine(dir1, file), Path.Combine(dir1, file)))));
            return new UpdateDirectory()
            {
                Created = creates,
                Updated = updates,
                Deleted = deleted
            };
        }

        public event EventHandler<TextChangedEvent> OnTextChanged;


        [Description("Событие изменения текста")]
        public class TextChangedEvent
        {
            public Dictionary<int, string> Before { get; set; } = new Dictionary<int, string>();
            public Dictionary<int, string> Current { get; set; } = new Dictionary<int, string>();
        }


        private int Counter = 0;
        public CompareProgram() 
        {
            this.OnTextChanged = (sender, evt) => {
                Counter++;
                WriteLine(sender);
            };
        }






         
        [Label("Выполнение инструкции через командную строку")]
        [Description("Выполнение операции через коммандную оболоочку в отдельном процессе.")]
        private string CmdExec(string command)
        {
            string result = "";
            command = command.ReplaceAll("\n", "").ReplaceAll("\r", "").ReplaceAll(@"\\", @"\").ReplaceAll(@"//", @"/");

            ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);

            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            string response = process.StandardOutput.ReadToEnd();
            result = response.ReplaceAll("\r", "\n");
            result = result.ReplaceAll("\n\n", "\n");
            while (result.EndsWith("\n"))
            {
                result = result.Substring(0, result.Length - 1);
            }
            process.WaitForExit();
            return result;
        }
         

    }


}



