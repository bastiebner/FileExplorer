namespace FileExplorer;

public interface IComponent
{
    string Name { get; }
    Folder? Parent { get; set; }

    string Read();
    void Delete();
    void Move(IComponent goal);
    IComponent Copy();
}
