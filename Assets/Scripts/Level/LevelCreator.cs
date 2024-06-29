using System.Collections.Generic;
using Managers.SaveLoadManagers;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private PlayerContainer _playerContainer;
    [SerializeField] private Level _testLevel;

    public void CreateLevel()
    {
        Level level;
        var playerContainer = Instantiate(_playerContainer);
        var lastLevelPath = PlayerSaveLoadManager.Instance.GetLastLevelData().lastLevelPath;
        
        if (lastLevelPath != "")
        {
            level = Instantiate(Resources.Load(lastLevelPath), transform, true).GetComponent<Level>();

            var playerPosition = PlayerSaveLoadManager.Instance.GetLastLevelData().lastPosition;
            var playerRotation = PlayerSaveLoadManager.Instance.GetLastLevelData().lastRotation;
            var playerTransform = playerContainer.GetPlayer().transform;
            playerTransform.position = new Vector3(playerPosition.x, playerPosition.y);
            playerTransform.Rotate(new Vector3(0,0, playerRotation));
        }
        else
        {
            level = Instantiate(_testLevel, transform, true);
            var exits = level.GetEntryExits();
            var entrance = exits[Random.Range(0, exits.Count)];
            
            playerContainer.GetPlayer().transform.position = new Vector3(entrance.transform.position.x, entrance.transform.position.y);

            var currentExitIndexes = new List<int>();
            var currentExits = entrance.GetCurrentExits();
            for (var i = 0; i < exits.Count; i++)
            {
                if (currentExits.Contains(exits[i]))
                {
                    currentExitIndexes.Add(i);
                }
            }
             
            var newLevelData = new LevelData
            {
                lastPosition = playerContainer.transform.position,
                exitIndexes = currentExitIndexes,
                lastLevelPath = $"Levels/{_testLevel.name}",
                lastRemainingMinutes = 10
            };

            PlayerSaveLoadManager.Instance.SetLastLevelData(newLevelData);
        }
        
        ChunksSaveLoadManager.Instance.SetLevelInfo(level.GetLootBoxIndexes, level.GetLootBoxes);
        GameBus.Instance.PlayerHumanoid = playerContainer.GetPlayer();
        GameBus.Instance.Level = level;
        level.Init();
    }
}
