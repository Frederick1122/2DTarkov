using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private PlayerContainer _playerContainer;
    [SerializeField] private Level _testLevel;

    private void Start()
    {
        CreateLevel();
    }

    private void CreateLevel()
    {
        var level = Instantiate(_testLevel, transform, true);
        var exits = level.GetEntryExits();
        var entrance = exits[Random.Range(0, exits.Count)];
        var playerContainer = Instantiate(_playerContainer);
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

        Player.Instance.SetExitIndexes(currentExitIndexes);
        GameBus.Instance.SetPlayer(playerContainer.GetPlayer());
        GameBus.Instance.SetLevel(level);
        Chunks.Instance.SetLevelInfo(level.GetLootBoxIndexes, level.GetLootBoxes);
    }
}
