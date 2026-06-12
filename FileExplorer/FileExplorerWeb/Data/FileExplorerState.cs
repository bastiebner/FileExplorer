using FileExplorer;

namespace FileExplorerWeb.Data;

public class FileExplorerState
{
    private readonly JsonStorageObserver storageObserver;

    public FileExplorerState(FileExplorerDataStore dataStore)
    {
        Root = dataStore.Load();
        Manager = new ComponentManager(Root);
        storageObserver = new JsonStorageObserver(dataStore);
        Manager.Attach(storageObserver);
    }

    public Folder Root { get; }
    public ComponentManager Manager { get; }
}
