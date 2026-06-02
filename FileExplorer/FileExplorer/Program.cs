namespace FileExplorer;

public static class Program
{
    public static void Main()
    {
        Folder root = new Folder("Root");
        ComponentManager manager = new ComponentManager(root);
        ExplorerView view = new ExplorerView();

        manager.Attach(view);

        Folder documents = new Folder("Documents");
        Folder pictures = new Folder("Pictures");
        Folder school = new Folder("School");

        IComponent notes = new File("notes.txt", "Hallo Welt");
        IComponent todo = new File("todo.txt", "Mathe lernen");
        IComponent image = new File("image.png", "Bilddaten");

        manager.Add(root, documents);
        manager.Add(root, pictures);
        manager.Add(root, school);

        manager.Add(documents, notes);
        manager.Add(documents, todo);
        manager.Add(pictures, image);

        manager.Move(todo, school);
        manager.Copy(pictures, school);
        manager.Delete(image);

        IComponent decoratedNotes = new ReadOnlyDecorator(notes);
        decoratedNotes = new FavoriteDecorator(decoratedNotes);
        decoratedNotes = new EncryptedDecorator(decoratedNotes);

        manager.Replace(documents, notes, decoratedNotes);

        Console.WriteLine("Kombinierte Decorator-Ausgabe:");
        Console.WriteLine(decoratedNotes.Read());

        try
        {
            manager.Delete(decoratedNotes);
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}
