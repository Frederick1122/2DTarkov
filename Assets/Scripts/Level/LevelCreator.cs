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
        var exits = level.GetExits();
        var currentExitPosition = exits[Random.Range(0, exits.Count)].transform.position;
        var playerContainer = Instantiate(_playerContainer);
        playerContainer.GetPlayer().transform.position = new Vector3(currentExitPosition.x, currentExitPosition.y);
        
        GameBus.Instance.SetPlayer(playerContainer.GetPlayer());
    }
}
