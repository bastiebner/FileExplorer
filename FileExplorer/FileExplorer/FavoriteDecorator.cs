namespace FileExplorer;

public class FavoriteDecorator : ComponentDecorator
{
    public FavoriteDecorator(IComponent component) : base(component)
    {
        favorite = true;
    }

    public override string Read()
    {
        return $"[Favorite] {component.Read()}";
    }

    public override IComponent Copy()
    {
        return new FavoriteDecorator(component.Copy());
    }
}
