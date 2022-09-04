public static class Extensions
{
    
    public static int ToInt(this string text)
    {
        return int.Parse(text.Trim()); ;
    }
    

    public static string ToJsonOnScreen(this object text)
    {
        return System.Text.Json.JsonSerializer.Serialize(text,new System.Text.Json.JsonSerializerOptions()
        {
            WriteIndented = true
        });
    }
    public static string ReplaceAll(this string text, string s1, string s2)
    {
        while (text.IndexOf(s1) != -1)
        {
            text = text.Replace(s1, s2);
        }
        return text;
    }
}



/******

bool first = true;
                            bool toCurrent = false;
                            bool toBefore = false;
                            var current = new Dictionary<int, string>();
                            var before = new Dictionary<int, string>();
                            string result = Comp(evt.FullPath, newpath);
                            foreach ( string line in result.Split("\n"))
                            {
                                
                                WriteLine(line);
                                if(first)
                                {
                                    first = false;
                                    continue;
                                }
                                if (line.StartsWith("***** "))
                                {
                                    //WriteLine(line.Substring("***** ".Length).Trim());
                                    //WriteLine(evt.FullPath);
                                    /*string parsedPath = l ine.Substring("***** ".Length).Trim();* /
                                    //if (evt.FullPath.ToUpper().Equals(parsedPath.ToUpper()))
                                    if (toCurrent == false)
                                    {
                                        toCurrent = true;
                                        toBefore = false;
                                    }
                                    else
                                    {
                                        toBefore = true;
                                        toCurrent = false;

                                    }
                                    continue;
                                }
                                else
                                {
                                    string text = line.Trim();
                                    int iint = text.IndexOf(":");
                                    if (iint != -1)
                                    {
                                        int pos = text.Substring(0, iint).ToInt();
                                        string value = text.Substring(text.IndexOf(":") + 1);
                                        current[pos] = value;
                                    }
                                   
                                }
                                

                            }

                            (new { before = before, current = current }).ToJsonOnScreen().WriteToConsole();

*/ 
