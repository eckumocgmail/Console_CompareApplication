using System.Collections.Generic;
/// <summary>
/// Событие изменения текста
/// </summary>
public class UpdateFile
{
    public Dictionary<int, string> before { get; set; } = new Dictionary<int, string>();
    public Dictionary<int, string> current { get; set; } = new Dictionary<int, string>();
    public string[] Items { get; set; }
}
