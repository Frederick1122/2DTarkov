using System;
using System.Collections.Generic;
using Base;
using InteractObjects;
using UnityEngine;

public class GameBus : Singleton<GameBus>
{
    [SerializeField] private PlayerHumanoid _playerHumanoid;
    [SerializeField] private InteractItem _baseItem;
    [SerializeField] private Joystick _joystick;

    public PlayerHumanoid GetPlayer()
    {
        if(_playerHumanoid == null)
            Debug.LogError("GameBus doesn't found player. Update this value");

        return _playerHumanoid;
    }

    public InteractItem GetBaseItem() => _baseItem;

    public Joystick GetJoystick() => _joystick;

    public void SetPlayer( PlayerHumanoid playerHumanoid ) => _playerHumanoid = playerHumanoid;
}