namespace FileExplorerWeb.Data;

public class ExplorerItemData
{
    public string Type { get; set; } = "File";
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool ReadOnly { get; set; }
    public bool Favorite { get; set; }
    public bool Encrypted { get; set; }
    public List<ExplorerItemData> Children { get; set; } = new List<ExplorerItemData>();
}
