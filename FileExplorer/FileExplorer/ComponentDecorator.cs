namespace FileExplorer;

public abstract class ComponentDecorator : IComponent
{
    protected readonly IComponent component;
    protected bool readOnly;
    protected bool favorite;
    protected bool encrypted;

    protected ComponentDecorator(IComponent component)
    {
        this.component = component;
    }

    public string Name => component.Name;

    public Folder? Parent
    {
        get => component.Parent;
        set => component.Parent = value;
    }

    public virtual string Read()
    {
        return component.Read();
    }

    public virtual void Delete()
    {
        if (HasReadOnlyDecorator())
        {
            throw new InvalidOperationException("Diese Datei ist schreibgeschuetzt.");
        }

        Parent?.Remove(this);
    }

    public virtual void Move(IComponent goal)
    {
        if (goal is not Folder targetFolder)
        {
            throw new InvalidOperationException("Components koennen nur in Ordner verschoben werden.");
        }

        Parent?.Remove(this);
        targetFolder.Add(this);
    }

    public virtual IComponent Copy()
    {
        return component.Copy();
    }

    protected string GetDecoratorText()
    {
        List<string> parts = new List<string>();
        CollectDecoratorTexts(parts);
        return string.Join(" ", parts);
    }

    private void CollectDecoratorTexts(List<string> parts)
    {
        if (component is ComponentDecorator decoratedComponent)
        {
            decoratedComponent.CollectDecoratorTexts(parts);
        }

        if (readOnly)
        {
            parts.Add("[ReadOnly]");
        }

        if (favorite)
        {
            parts.Add("[Favorite]");
        }

        if (encrypted)
        {
            parts.Add("[Encrypted]");
        }
    }

    private bool HasReadOnlyDecorator()
    {
        if (readOnly)
        {
            return true;
        }

        if (component is ComponentDecorator decoratedComponent)
        {
            return decoratedComponent.HasReadOnlyDecorator();
        }

        return false;
    }
}
