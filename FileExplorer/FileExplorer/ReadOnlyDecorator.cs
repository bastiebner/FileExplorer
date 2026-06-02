namespace FileExplorer;

public class ReadOnlyDecorator : ComponentDecorator
{
    public ReadOnlyDecorator(IComponent component) : base(component)
    {
        readOnly = true;
    }

    public override string Read()
    {
        return $"[ReadOnly] {component.Read()}";
    }

    public override void Delete()
    {
        throw new InvalidOperationException("Diese Datei ist schreibgeschuetzt.");
    }

    public override IComponent Copy()
    {
        return new ReadOnlyDecorator(component.Copy());
    }
}
