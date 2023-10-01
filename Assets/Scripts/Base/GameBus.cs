using System;
using Base;
using InteractObjects;
using NavMeshPlus.Components;
using UnityEngine;

public class GameBus : Singleton<GameBus>
{
    public event Action<Level> OnLevelSet;
    
    [SerializeField] private PlayerHumanoid _playerHumanoid;
    [SerializeField] private Level _level;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private NavMeshSurface _navMeshSurface;

    public PlayerHumanoid GetPlayer()
    {
        if(_playerHumanoid == null)
            Debug.LogError("GameBus doesn't found player. Update this value");

        return _playerHumanoid;
    }

    public Joystick GetJoystick()
    {
        if(_joystick == null || _joystick == default)
            _joystick = FindObjectOfType<Joystick>();
        
        if(_joystick == null)
            Debug.LogError("GameBus doesn't found joystick");

        return _joystick;
    }

    public Level GetLevel()
    {
        return _level;
    }
    
    public NavMeshSurface GetNavMeshSurface()
    {
        return _navMeshSurface;
    }
    
    public void SetPlayer( PlayerHumanoid playerHumanoid )
    {
        _playerHumanoid = playerHumanoid;
    }

    public void SetLevel(Level currentLevel)
    {
        _level = currentLevel;
        OnLevelSet?.Invoke(_level);
    }

    public void SetNavMeshSurface(NavMeshSurface navMeshSurface)
    {
        _navMeshSurface = navMeshSurface;
    }
}