using System;
using System.Collections.Generic;
using Base;
using UnityEditor;
using UnityEngine;

public class Chunks : SaveLoadManager<ChunksData, Chunks>
{
    private const string CHUNK_INFO_JSON_PATH = "GameChunkInfo.json";

    [Header("Chunk Preset")] [SerializeField]
    private Vector2 _chunkTableSize;

    [SerializeField] private float _chunkSideSize;
    [SerializeField] private Vector3 _chunkOffset;

    [Space(5)] [Header("Chunks")] [SerializeField]
    private List<int> _lootBoxIndexes;
    [SerializeField] private List<SaveInChunkLootBox> _saveInChunkLootBoxes;
    
    
    private Dictionary<int, SaveInChunkLootBox> _lootBoxes = new Dictionary<int, SaveInChunkLootBox>();

    public float GetChunkSideSize() => _chunkSideSize;

    public Vector2 GetChunkTableSize() => _chunkTableSize;

    public Vector3 GetChunkOffset() => _chunkOffset;

    public void SaveLootBox(int index, List<Item> items)
    {
        var flag = true;
        foreach (var lootBoxData in _saveData.lootBoxes)
        {
            if(lootBoxData.index != index)
                continue;
            
            lootBoxData.lootItems = items;
            flag = false;
            break;
        }

        if (flag)
        {
            var newLootBoxData = new LootBoxData
            {
                index = index,
                lootItems = items
            };
            _saveData.lootBoxes.Add(newLootBoxData);
        }
        
        Save();
    }

    protected override void Start()
    {
        base.Start();
        LoadLootBoxes();
    }

    private void LoadLootBoxes()
    {
        _lootBoxes.Clear();
        
        for (var i = 0; i < _lootBoxIndexes.Count; i++) 
            _lootBoxes.Add(_lootBoxIndexes[i], _saveInChunkLootBoxes[i]);

        foreach (var lootBoxData in _saveData.lootBoxes) 
            _lootBoxes[lootBoxData.index].Load(lootBoxData.lootItems);
    }

    protected override void Load()
    {
        base.Load();
        if (_saveData == null)
        {
            _saveData = new ChunksData();
            Save();
        }
    }
    
#if UNITY_EDITOR
    
    [ContextMenu("GenerateChunks")]
    private void GenerateChunks()
    {
        _saveData = new ChunksData();
        
        if (_chunkTableSize.x < 1 || _chunkTableSize.y < 1 || _chunkSideSize <= 0)
        {
            Debug.LogError("Chunk preset is wrong");
            return;
        }

        var allLootBoxes = new List<SaveInChunkLootBox>();
        allLootBoxes.AddRange(FindObjectsOfType<SaveInChunkLootBox>());

        if (allLootBoxes.Count == 0)
            Debug.LogError("SaveObjects not found");

        _saveInChunkLootBoxes = new List<SaveInChunkLootBox>();
        _lootBoxIndexes = new List<int>();
        
        _chunkTableSize = new Vector2((int) _chunkTableSize.x, (int) _chunkTableSize.y);
        var downVector = new Vector3(0, -_chunkSideSize);
        var rightVector = new Vector3(_chunkSideSize, 0);

        var saveLootBoxesInChunk = new List<SaveInChunkLootBox>();
        var counter = 1;
        for (var i = 0; i < _chunkTableSize.y; i++)
        {
            for (var j = 0; j < _chunkTableSize.x; j++)
            {
                saveLootBoxesInChunk.Clear();

                for (var index = allLootBoxes.Count - 1; index >= 0; index--)
                {
                    var objPosition = allLootBoxes[index].transform.position;
                    var leftUp = _chunkOffset + i * downVector + j * rightVector;
                    var rightDown = _chunkOffset + (i + 1) * downVector + (j + 1) * rightVector;

                    if (!(objPosition.x > leftUp.x && objPosition.x < rightDown.x))
                        continue;
                    if (!(objPosition.y < leftUp.y && objPosition.y > rightDown.y))
                        continue;

                    saveLootBoxesInChunk.Add(allLootBoxes[index]);
                    allLootBoxes.RemoveAt(index);
                }

                for (var k = 0; k < saveLootBoxesInChunk.Count; k++)
                {
                    var index = counter * 100000 + 1 * 10000 + k;
                    saveLootBoxesInChunk[k].SetIndex(index);
                    _lootBoxIndexes.Add(index);
                    _saveInChunkLootBoxes.Add(saveLootBoxesInChunk[k]);
                }
                
                counter++;
            }
        }
        
        Save();
        EditorUtility.SetDirty(this);
    }
    
#endif
    protected override void UpdatePath()
    {
        _secondPath = CHUNK_INFO_JSON_PATH;
        base.UpdatePath();
    }
}

[Serializable]
public class ChunksData
{
    public List<LootBoxData> lootBoxes = new List<LootBoxData>();
}

[Serializable]

public class LootBoxData
{
    public int index;
    public List<Item> lootItems = new List<Item>();
}

#if UNITY_EDITOR

[CustomEditor(typeof(Chunks))]
public class ChunkEditor : Editor
{
    public void OnSceneGUI()
    {
        var chunkManager = target as Chunks;

        if (chunkManager == null)
            return;

        var color = new Color(1, 0.8f, 0.4f, 1);
        var chunkTableSize = chunkManager.GetChunkTableSize();
        var chunkSideSize = chunkManager.GetChunkSideSize();
        var chunkOffset = chunkManager.GetChunkOffset();

        if (chunkTableSize.x < 1 || chunkTableSize.y < 1 || chunkSideSize <= 0)
            return;

        chunkTableSize = new Vector2((int) chunkTableSize.x, (int) chunkTableSize.y);

        Handles.color = color;

        var downVector = new Vector3(0, -chunkSideSize);
        var rightVector = new Vector3(chunkSideSize, 0);

        for (var i = 0; i < chunkTableSize.x + 1; i++)
        {
            var startPoint = chunkOffset + i * rightVector;
            Handles.DrawLine(startPoint, startPoint + downVector * chunkTableSize.y, 1.0f);
        }

        for (var i = 0; i < chunkTableSize.y + 1; i++)
        {
            var startPoint = chunkOffset + i * downVector;
            Handles.DrawLine(startPoint, startPoint + rightVector * chunkTableSize.x, 1.0f);
        }

        GUI.color = color;

        var counter = 1;
        for (var i = 0; i < chunkTableSize.y; i++)
        {
            for (var j = 0; j < chunkTableSize.x; j++)
            {
                Handles.Label(chunkOffset + i * downVector + j * rightVector + new Vector3(0.1f, -0.1f),
                    $"Chunk {counter++}");
            }
        }
    }
}

#endif