namespace FileExplorer;

public class File : IComponent
{
    private readonly string content;

    public File(string name, string content)
    {
        Name = name;
        this.content = content;
    }

    public string Name { get; }
    public Folder? Parent { get; set; }

    public string Read()
    {
        return $"{Name}: {content}";
    }

    public void Delete()
    {
        Parent?.Remove(this);
    }

    public void Move(IComponent goal)
    {
        if (goal is not Folder targetFolder)
        {
            throw new InvalidOperationException("Dateien koennen nur in Ordner verschoben werden.");
        }

        Parent?.Remove(this);
        targetFolder.Add(this);
    }

    public IComponent Copy()
    {
        return new File($"{Name} - Copy", content);
    }
}
