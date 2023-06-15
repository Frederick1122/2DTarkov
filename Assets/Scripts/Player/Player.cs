using System;
using Base;
using UnityEngine;

public class Player : SaveLoadManager<PlayerData, Player>
{
    private const string PLAYER_JSON_PATH = "Player.json";
    
    public event Action<int> OnHpChanged;

    [ContextMenu("Clear Player Data")]
    private void ClearPlayerData()
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
    
    public void SetLastLevelData(LastLevelData levelData)
    {
        _saveData.lastLevelData = levelData;
        Save();
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
    public LastLevelData lastLevelData;

    public PlayerData(int hp = 100)
    {
        this.hp = hp;
    }
}

[Serializable]
public class LastLevelData
{
    public string lastLevelPath = "";
    public Vector3 lastPosition;
    public int remainingMinutes;
    public int remainingSeconds;
}
