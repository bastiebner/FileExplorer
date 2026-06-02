namespace FileExplorer;

public class EncryptedDecorator : ComponentDecorator
{
    public EncryptedDecorator(IComponent component) : base(component)
    {
        encrypted = true;
    }

    public override string Read()
    {
        return $"{GetDecoratorText()} {Name}: Access denied";
    }

    public override IComponent Copy()
    {
        return new EncryptedDecorator(component.Copy());
    }
}
