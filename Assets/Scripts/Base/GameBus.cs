using Base;
using InteractObjects;
using UnityEngine;

public class GameBus : Singleton<GameBus>
{
    [SerializeField] private Player _player;
    [SerializeField] private InteractItem _baseItem;
    [SerializeField] private Joystick _joystick;
    
    public Player GetPlayer()
    {
        if(_player == null)
            Debug.LogError("GameBus doesn't found player. Update this value");

        return _player;
    }

    public InteractItem GetBaseItem() => _baseItem;

    public Joystick GetJoystick() => _joystick;

    public void SetPlayer( Player player ) => _player = player;
}