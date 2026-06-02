using System.Text;

namespace FileExplorer;

public class Folder : IComponent
{
    private readonly List<IComponent> childComponents;

    public Folder(string name)
    {
        Name = name;
        childComponents = new List<IComponent>();
    }

    public string Name { get; }
    public Folder? Parent { get; set; }

    public void Add(IComponent comp)
    {
        comp.Parent?.Remove(comp);
        childComponents.Add(comp);
        comp.Parent = this;
    }

    public void Remove(IComponent comp)
    {
        if (childComponents.Remove(comp))
        {
            comp.Parent = null;
        }
    }

    public void Replace(IComponent oldComp, IComponent newComp)
    {
        int index = childComponents.IndexOf(oldComp);

        if (index == -1)
        {
            throw new InvalidOperationException($"{oldComp.Name} wurde in {Name} nicht gefunden.");
        }

        oldComp.Parent = null;
        newComp.Parent?.Remove(newComp);
        childComponents[index] = newComp;
        newComp.Parent = this;
    }

    public string Read()
    {
        StringBuilder builder = new StringBuilder();
        Read(builder, 0);
        return builder.ToString();
    }

    public void Delete()
    {
        Parent?.Remove(this);
    }

    public void Move(IComponent goal)
    {
        if (goal is not Folder targetFolder)
        {
            throw new InvalidOperationException("Ordner koennen nur in andere Ordner verschoben werden.");
        }

        Parent?.Remove(this);
        targetFolder.Add(this);
    }

    public IComponent Copy()
    {
        Folder copy = new Folder($"{Name} - Copy");

        foreach (IComponent child in childComponents)
        {
            copy.Add(child.Copy());
        }

        return copy;
    }

    private void Read(StringBuilder builder, int level)
    {
        builder.AppendLine($"{GetIndent(level)}[Folder] {Name}");

        foreach (IComponent child in childComponents)
        {
            string childText = child.Read();
            string[] lines = childText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                builder.AppendLine($"{GetIndent(level + 1)}{line}");
            }
        }
    }

    private static string GetIndent(int level)
    {
        return new string(' ', level * 2);
    }
}
