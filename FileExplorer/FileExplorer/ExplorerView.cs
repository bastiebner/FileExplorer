namespace FileExplorer;

public class ExplorerView : IObserver
{
    public void Update(string message, IComponent root)
    {
        Console.WriteLine(message);
        Console.WriteLine(root.Read());
    }
}
