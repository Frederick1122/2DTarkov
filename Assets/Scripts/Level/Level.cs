using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<EntryExit> _entryExits = new List<EntryExit>();
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [Space(5)]
    [SerializeField] private List<Enemy> _enemies = new List<Enemy>();

    [Space(5)] [Header("Chunks")] 
    [SerializeField] private List<int> _lootBoxIndexes;
    [SerializeField] private List<SaveInChunkLootBox> _saveInChunkLootBoxes;
    
    public List<EntryExit> GetEntryExits() => _entryExits;

    public NavMeshSurface GetNavMeshSurface() => _navMeshSurface;
    
    public List<SaveInChunkLootBox> GetLootBoxes => _saveInChunkLootBoxes;
    
    public List<int> GetLootBoxIndexes => _lootBoxIndexes;

    public void Init()
    {
        InitEnemies();
    }
    
    public void SetLootBoxes(List<SaveInChunkLootBox> lootBoxes, List<int> lootBoxIndexes)
    {
        _saveInChunkLootBoxes = lootBoxes;
        _lootBoxIndexes = lootBoxIndexes;
    }
    
    private void InitEnemies()
    {
        foreach (var enemy in _enemies) 
            enemy.Init();
    }
}
