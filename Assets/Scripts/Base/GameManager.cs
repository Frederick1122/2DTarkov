using Base;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player _player;

    public Player GetPlayer() => _player;
    
    private void OnValidate() => UpdateFields();

    private void Start()
    {
        UpdateFields();
    }

    private void UpdateFields()
    {
        if (_player == null || _player == default)
        {
            _player = FindObjectOfType<Player>();

            if (_player == null || _player == default)
                Debug.LogError("GameManager doesn't found player. Update this value");
        }
    }
}
