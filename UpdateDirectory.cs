using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UpdateDirectory
{
    public List<string> Deleted { get; set; }
    public Dictionary<string,string> Created { get; set; }
    public Dictionary<string,UpdateFile> Updated { get; set; }
}

