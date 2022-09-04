using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 
internal class Program
{
    internal static void Main(string[] args) 
        => CompareDirectoriesProgram.Run(new string[] {
            @"D:\NetProjects2024\_Data\BusinessResources\DataADO",
            @"D:\Projects\Console_AppProgram\ADO\"
        }); 
}
 
