using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<EntryExit> _entryExits = new List<EntryExit>();
    [Space(5)] [Header("Chunks")] 
    [SerializeField] private List<int> _lootBoxIndexes;
    [SerializeField] private List<SaveInChunkLootBox> _saveInChunkLootBoxes;
    
    public List<EntryExit> GetEntryExits() => _entryExits;

    public List<SaveInChunkLootBox> GetLootBoxes => _saveInChunkLootBoxes;
    public List<int> GetLootBoxIndexes => _lootBoxIndexes;

    public void SetLootBoxes(List<SaveInChunkLootBox> lootBoxes, List<int> lootBoxIndexes)
    {
        _saveInChunkLootBoxes = lootBoxes;
        _lootBoxIndexes = lootBoxIndexes;
    }
}
