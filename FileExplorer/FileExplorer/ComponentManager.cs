namespace FileExplorer;

public class ComponentManager
{
    private readonly IComponent root;
    private readonly List<IObserver> observers;

    public ComponentManager(IComponent root)
    {
        this.root = root;
        observers = new List<IObserver>();
    }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify(string message)
    {
        foreach (IObserver observer in observers)
        {
            observer.Update(message, root);
        }
    }

    public void Add(Folder parent, IComponent component)
    {
        parent.Add(component);
        Notify($"{component.Name} wurde hinzugefuegt.");
    }

    public void Delete(IComponent component)
    {
        component.Delete();
        Notify($"{component.Name} wurde geloescht.");
    }

    public void Move(IComponent component, IComponent goal)
    {
        component.Move(goal);
        Notify($"{component.Name} wurde verschoben.");
    }

    public void Copy(IComponent component, Folder goal)
    {
        IComponent copy = component.Copy();
        goal.Add(copy);
        Notify($"{component.Name} wurde kopiert.");
    }

    public void Replace(Folder parent, IComponent oldComponent, IComponent newComponent)
    {
        parent.Replace(oldComponent, newComponent);
        Notify($"{oldComponent.Name} wurde dekoriert.");
    }
}
