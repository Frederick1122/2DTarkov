using Base;
using InteractObjects;
using UnityEngine;

public class GameBus : Singleton<GameBus>
{
    [SerializeField] private Player _player;
    [SerializeField] private InteractItem _baseItem;
    
    public Player GetPlayer() => _player;

    public InteractItem GetBaseItem() => _baseItem;
    
    private void OnValidate() => UpdateFields();

    private void Start() => UpdateFields();
    
    private void UpdateFields()
    {
        if (_player == null || _player == default)
        {
            _player = FindObjectOfType<Player>();

            if (_player == null || _player == default)
                Debug.LogError("GameBus doesn't found player. Update this value");
        }
    }
}