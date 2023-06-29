using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_CompareApplication.CompareModule
{
    public class CompareFiles
    {

        public static UpdateFile CompareTextFiles( string path1, string path2)
        {
            if (!System.IO.File.Exists(path1) || !System.IO.File.Exists(path2))
                throw new ArgumentException("Убедитесь что в качестве аргументов передаёте путь к доступным файлам");
            string message = CmdExec(@$"fc /N ""{path1}"" ""{path2}""");

            UpdateFile result = new UpdateFile();
            int sepCount = 0;
            string filename = "";
            string[] lines = message.ReplaceAll("\r", "\n").ReplaceAll("\n\n", "\n").Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {

                if (lines[i].StartsWith("***** "))
                {
                    filename = (lines[i].Substring("***** ".Length).Trim());
                    sepCount++;
                    continue;
                }

                if (sepCount == 1)
                {
                    Console.WriteLine($"f1 {lines[i]}");
                    string text = lines[i].Trim();
                    int pos = text.Substring(0, text.IndexOf(":")).ToInt();
                    string value = text.Substring(text.IndexOf(":") + 1);
                    result.before[pos] = value;
                }
                else if (sepCount == 2)
                {
                    Console.WriteLine($"f2 {lines[i]}");
                    string text = lines[i].Trim();
                    int pos = text.Substring(0, text.IndexOf(":")).ToInt();
                    string value = text.Substring(text.IndexOf(":") + 1);
                    result.current[pos] = value;
                }
            }

            foreach (var line in result.Items)
                Console.WriteLine(line);

            JsonConvert.SerializeObject(new { result.before, result.current }, Formatting.Indented);
            return result;
        }

        public static UpdateFile CompareText(string text1, string text2)
        {
            string filename1 = "";
            string filename2 = "";
            System.IO.File.WriteAllText(filename1,text1);
            System.IO.File.WriteAllText(filename2, text2);
            return CompareTextFiles(filename1,filename2);   

        }

        /// <summary>
        /// Выполнение инструкции через командную строку
        /// </summary>
        private static string CmdExec(string command)
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
