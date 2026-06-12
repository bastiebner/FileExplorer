using FileExplorer;

namespace FileExplorerWeb.Data;

public class JsonStorageObserver : IObserver
{
    private readonly FileExplorerDataStore dataStore;

    public JsonStorageObserver(FileExplorerDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public void Update(string message, IComponent root)
    {
        dataStore.Save(root);
    }
}
