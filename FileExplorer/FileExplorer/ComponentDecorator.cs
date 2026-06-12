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
    public IComponent WrappedComponent => component;

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
        if (HasReadOnly())
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

        if (Unwrap(component) is Folder sourceFolder
            && (sourceFolder == targetFolder || sourceFolder.ContainsFolder(targetFolder)))
        {
            throw new InvalidOperationException("Ein Ordner kann nicht in sich selbst verschoben werden.");
        }

        Parent?.Remove(this);
        targetFolder.Add(this);
    }

    public virtual IComponent Copy()
    {
        return component.Copy();
    }

    public bool HasReadOnly()
    {
        return HasDecorator(decorator => decorator.readOnly);
    }

    public bool HasFavorite()
    {
        return HasDecorator(decorator => decorator.favorite);
    }

    public bool HasEncrypted()
    {
        return HasDecorator(decorator => decorator.encrypted);
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

    private bool HasDecorator(Func<ComponentDecorator, bool> check)
    {
        if (check(this))
        {
            return true;
        }

        if (component is ComponentDecorator decoratedComponent)
        {
            return decoratedComponent.HasDecorator(check);
        }

        return false;
    }

    private static IComponent Unwrap(IComponent source)
    {
        while (source is ComponentDecorator decorator)
        {
            source = decorator.WrappedComponent;
        }

        return source;
    }
}
