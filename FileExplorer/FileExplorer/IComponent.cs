namespace FileExplorer;

public interface IComponent
{
    public string Name { get; set; }
    public string Read();
    public void Delete();
    public void Move(IComponent goal);
    public IComponent Copy();
}