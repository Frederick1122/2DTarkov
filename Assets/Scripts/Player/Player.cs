using System;
using System.Collections.Generic;
using Base;
using UnityEngine;

public class Player : SaveLoadManager<PlayerData, Player>
{
    private const string PLAYER_JSON_PATH = "Player.json";
    
    public event Action<int> OnHpChanged;

    [ContextMenu("Clear Player Data")]
    public void ClearPlayerData()
    {
        _saveData = new PlayerData();
        Save();
    }
    
    public void ChangeHp(int change)
    {
        _saveData.hp = Mathf.Clamp( _saveData.hp + change, 0, 100);
        OnHpChanged?.Invoke(_saveData.hp);
        Save();
    }

    public int GetHp()
    {
        return _saveData.hp;
    }

    public void SetPlayerTransformData(Vector3 playerPosition, float playerRotation)
    {
        _saveData.levelData.lastPosition = playerPosition;
        _saveData.levelData.lastRotation = playerRotation;
        Save();
    }
    
    public void SetLastLevelData(LevelData levelData)
    {
        _saveData.levelData = levelData;
        Save();
    }

    public void SetLastRemainingTime(TimeSpan timeSpan)
    {
        _saveData.levelData.lastRemainingMinutes = timeSpan.Minutes;
        _saveData.levelData.lastRemainingSeconds = timeSpan.Seconds;
        Save();
    }

    public LevelData GetLastLevelData()
    {
        return _saveData.levelData;
    }
    
    protected override void Load()
    {
        base.Load();
        if (_saveData == null)
        {
            _saveData = new PlayerData();
            Save();
        }
    }
    
    protected override void UpdatePath()
    {
        _secondPath = PLAYER_JSON_PATH;
        base.UpdatePath();
    }
}

[Serializable]
public class PlayerData
{
    private const string LEVEL_DIRECTORY = "";
    
    public int hp;
    public LevelData levelData;

    public PlayerData(int hp = 100)
    {
        this.hp = hp;
    }
}

[Serializable]
public class LevelData
{
    public string lastLevelPath = "";
    
    public Vector3 lastPosition;
    public float lastRotation;
    
    public int lastRemainingMinutes;
    public int lastRemainingSeconds;

    public List<int> exitIndexes;
}
