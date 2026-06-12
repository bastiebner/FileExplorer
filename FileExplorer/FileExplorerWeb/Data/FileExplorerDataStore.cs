using System.Text.Json;
using FileExplorer;
using ExplorerFile = FileExplorer.File;

namespace FileExplorerWeb.Data;

public class FileExplorerDataStore
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private readonly string filePath;

    public FileExplorerDataStore(IWebHostEnvironment environment)
    {
        filePath = Path.Combine(environment.ContentRootPath, "Data", "explorer-data.json");
    }

    public Folder Load()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        if (!System.IO.File.Exists(filePath))
        {
            Folder root = CreateDefaultRoot();
            Save(root);
            return root;
        }

        string json = System.IO.File.ReadAllText(filePath);
        ExplorerItemData? data = JsonSerializer.Deserialize<ExplorerItemData>(json, JsonOptions);

        if (data is null)
        {
            Folder root = CreateDefaultRoot();
            Save(root);
            return root;
        }

        return ToComponent(data) as Folder ?? CreateDefaultRoot();
    }

    public void Save(IComponent root)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        ExplorerItemData data = ToData(root);
        string json = JsonSerializer.Serialize(data, JsonOptions);
        System.IO.File.WriteAllText(filePath, json);
    }

    private static Folder CreateDefaultRoot()
    {
        Folder root = new Folder("Root");
        Folder documents = new Folder("Documents");
        Folder pictures = new Folder("Pictures");
        Folder school = new Folder("School");

        documents.Add(new ExplorerFile("notes.txt", "Hallo Welt"));
        documents.Add(new ExplorerFile("todo.txt", "Mathe lernen"));
        pictures.Add(new ExplorerFile("image.png", "Bilddaten"));

        root.Add(documents);
        root.Add(pictures);
        root.Add(school);

        return root;
    }

    private static IComponent ToComponent(ExplorerItemData data)
    {
        IComponent component;

        if (data.Type == "Folder")
        {
            Folder folder = new Folder(data.Name);

            foreach (ExplorerItemData child in data.Children)
            {
                folder.Add(ToComponent(child));
            }

            component = folder;
        }
        else
        {
            component = new ExplorerFile(data.Name, data.Content);
        }

        return ApplyDecorators(component, data);
    }

    private static IComponent ApplyDecorators(IComponent component, ExplorerItemData data)
    {
        if (data.ReadOnly)
        {
            component = new ReadOnlyDecorator(component);
        }

        if (data.Favorite)
        {
            component = new FavoriteDecorator(component);
        }

        if (data.Encrypted)
        {
            component = new EncryptedDecorator(component);
        }

        return component;
    }

    private static ExplorerItemData ToData(IComponent source)
    {
        ExplorerItemData data = new ExplorerItemData();
        IComponent component = source;

        if (source is ComponentDecorator decorator)
        {
            data.ReadOnly = decorator.HasReadOnly();
            data.Favorite = decorator.HasFavorite();
            data.Encrypted = decorator.HasEncrypted();
            component = Unwrap(source);
        }

        if (component is Folder folder)
        {
            data.Type = "Folder";
            data.Name = folder.Name;
            data.Children = folder.ChildComponents.Select(ToData).ToList();
            return data;
        }

        if (component is ExplorerFile file)
        {
            data.Type = "File";
            data.Name = file.Name;
            data.Content = file.Content;
            return data;
        }

        data.Type = "File";
        data.Name = component.Name;
        data.Content = component.Read();
        return data;
    }

    private static IComponent Unwrap(IComponent component)
    {
        while (component is ComponentDecorator decorator)
        {
            component = decorator.WrappedComponent;
        }

        return component;
    }
}
