using System;
using System.Collections.Generic;
using Base;
using InteractObjects;
using UnityEngine;

public class GameBus : Singleton<GameBus>
{
    [SerializeField] private PlayerHumanoid _playerHumanoid;
    [SerializeField] private Level _level;
    [SerializeField] private InteractItem _baseItem;
    [SerializeField] private Joystick _joystick;

    public PlayerHumanoid GetPlayer()
    {
        if(_playerHumanoid == null)
            Debug.LogError("GameBus doesn't found player. Update this value");

        return _playerHumanoid;
    }

    public InteractItem GetBaseItem()
    {
        return _baseItem;
    }

    public Joystick GetJoystick()
    {
        return _joystick;
    }

    public Level GetLevel()
    {
        return _level;
    }
    
    public void SetPlayer( PlayerHumanoid playerHumanoid )
    {
        _playerHumanoid = playerHumanoid;
    }

    public void SetLevel(Level currentLevel)
    {
        _level = currentLevel;
    }
}