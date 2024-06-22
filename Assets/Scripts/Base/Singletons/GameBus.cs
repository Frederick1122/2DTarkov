using System;
using Base;
using NavMeshPlus.Components;
using Player.InputSystem;
using UnityEngine;

public class GameBus : Singleton<GameBus>
{
    public event Action<Level> OnLevelSet;

    public PlayerHumanoid PlayerHumanoid
    {
        get
        {   
            if(_playerHumanoid == null || _playerHumanoid == default)
                _playerHumanoid = FindObjectOfType<PlayerHumanoid>();
    
            if(_playerHumanoid == null)
                Debug.LogError("GameBus doesn't found player. Update this value");

            return _playerHumanoid;
        }
        set => _playerHumanoid = value;
    }

    public Level Level
    {
        get
        {
            if (_level == null || _level == default) 
                _level = FindObjectOfType<Level>();

            if(_level == null)
                Debug.LogError("GameBus doesn't found Level");
            
            return _level;
        }
        set
        {
            _level = value;
            OnLevelSet?.Invoke(_level);
        }
    }

    public NavMeshSurface NavMeshSurface
    {
        get
        {
            if (_navMeshSurface == null || _navMeshSurface == default)
            {
                _navMeshSurface = _level.GetNavMeshSurface();
            }
            
            return _navMeshSurface;
        }
    }

    public Joystick Joystick
    {
        get
        {
            if(_joystick == null || _joystick == default)
                _joystick = FindObjectOfType<Joystick>();
        
            if(_joystick == null)
                Debug.LogError("GameBus doesn't found joystick");
            return _joystick;
        }
    }

    public PlayerInputSystem PlayerInputSystem
    {
        get
        {
            if (_playerInputSystem == null) 
                _playerInputSystem = gameObject.AddComponent<PlayerInputSystem>();

            return _playerInputSystem;
        }
    }

    private PlayerHumanoid _playerHumanoid;
    private Level _level;
    private Joystick _joystick;
    private NavMeshSurface _navMeshSurface;
    private PlayerInputSystem _playerInputSystem;
}