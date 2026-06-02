namespace FileExplorer;

public interface IObserver
{
    public void Update(string message, IComponent root);
}