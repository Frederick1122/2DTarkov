using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    private const string BASE_SCENE_NAME = "MainMenu";
    
    [SerializeField] private string _necessarySceneName;
    
    void Start()
    {
        var currentScene = _necessarySceneName != "" ? _necessarySceneName : BASE_SCENE_NAME;
       
        var gameBusGenerator = gameObject.AddComponent<GameBusGenerator>();
        gameBusGenerator.Init();

        var sceneLoaderGenerator = gameObject.AddComponent<SceneLoaderGenerator>();
        sceneLoaderGenerator.Init();

        var playerGenerator = gameObject.AddComponent<PlayerGenerator>();
        playerGenerator.Init();

        var equipmentGenerator = gameObject.AddComponent<EquipmentGenerator>();
        equipmentGenerator.Init();
        
        var inventoryGenerator = gameObject.AddComponent<InventoryGenerator>();
        inventoryGenerator.Init();

        var chunksGenerator = gameObject.AddComponent<ChunksGenerator>();
        chunksGenerator.Init();

        SceneManager.LoadScene(currentScene);
    }
}

public class ManagerGenerator<T> : MonoBehaviour where T : MonoBehaviour
{
    public void Init()
    {
        var gameObjectWithManager = Instantiate(new GameObject());
        gameObjectWithManager.name = $"{typeof(T).Name}";
        gameObjectWithManager.AddComponent<T>();
        DontDestroyOnLoad(gameObjectWithManager);
    }
}

public class GameBusGenerator : ManagerGenerator<GameBus> {}
public class SceneLoaderGenerator : ManagerGenerator<SceneLoader> {}
public class PlayerGenerator : ManagerGenerator<PlayerSaveLoadManager> {}
public class EquipmentGenerator : ManagerGenerator<EquipmentSaveLoadManager> {}
public class InventoryGenerator : ManagerGenerator<InventorySaveLoadManager> {}
public class ChunksGenerator : ManagerGenerator<ChunksSaveLoadManager> {}