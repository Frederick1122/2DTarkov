using System.Collections.Generic;
using Base;
using InteractObjects;
using UnityEditor;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player _player;
    [SerializeField] private InteractItem _baseItem;
    [SerializeField] private ItemList _itemList;

    [Header("Chunk Preset")] 
    [SerializeField] private Vector2 _chunkTableSize;
    [SerializeField] private float _chunkSideSize;
    [SerializeField] private Vector3 _chunkOffset;
    [Space(5)]
    [Header("Chunks")] [SerializeField] private List<Chunk> _chunks;
    public float GetChunkSideSize() => _chunkSideSize;
    
    public Vector2 GetChunkTableSize() => _chunkTableSize;
    
    public Vector3 GetChunkOffset() => _chunkOffset;
    
    public Player GetPlayer() => _player;

    public InteractItem GetBaseItem() => _baseItem;
    
    private void OnValidate() => UpdateFields();

    private void Start() => UpdateFields();

    [ContextMenu("GenerateChunks")]
    private void GenerateChunks()
    {
        _chunks.Clear();

        if (_chunkTableSize.x < 1 || _chunkTableSize.y < 1 || _chunkSideSize <= 0)
        {
            Debug.LogError("Chunk preset is wrong");
            return;
        }

        var allSaveObjects = new List<SaveInChunk>();
        allSaveObjects.AddRange(FindObjectsOfType<SaveInChunk>());
        
        if(allSaveObjects.Count == 0) 
            Debug.LogError("SaveObjects not found");

        _chunkTableSize = new Vector2((int) _chunkTableSize.x, (int) _chunkTableSize.y);
        var downVector = new Vector3(0, -_chunkSideSize);
        var rightVector = new Vector3(_chunkSideSize, 0);
        
        var saveObjectsInChunk = new List<SaveInChunk>();
        var counter = 1;
        for (var i = 0; i < _chunkTableSize.y; i++)
        {
            for (var j = 0; j < _chunkTableSize.x ; j++)
            {
                var newChunk = new Chunk();
                saveObjectsInChunk.Clear();

                for (var index = allSaveObjects.Count - 1; index >= 0; index--)
                {
                    var objPosition = allSaveObjects[index].transform.position;
                    var leftUp = _chunkOffset + i * downVector + j * rightVector;
                    var rightDown = _chunkOffset + (i + 1) * downVector + (j + 1) * rightVector;
                    
                    if(!(objPosition.x > leftUp.x && objPosition.x < rightDown.x))
                        continue;
                    if(!(objPosition.y < leftUp.y && objPosition.y > rightDown.y))
                        continue;
            
                    saveObjectsInChunk.Add(allSaveObjects[index]);
                    allSaveObjects.RemoveAt(index);
                }
                
                newChunk.Init(counter++, saveObjectsInChunk);
                _chunks.Add(newChunk);
            }
        }
    }
    
    private void UpdateFields()
    {
        if (_player == null || _player == default)
        {
            _player = FindObjectOfType<Player>();

            if (_player == null || _player == default)
                Debug.LogError("GameManager doesn't found player. Update this value");
        }
    }
}

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public void OnSceneGUI()
    {
        var gameManager = target as GameManager;

        if (gameManager == null)
            return;

        var color = new Color(1, 0.8f, 0.4f, 1);
        var chunkTableSize = gameManager.GetChunkTableSize();
        var chunkSideSize = gameManager.GetChunkSideSize();
        var chunkOffset = gameManager.GetChunkOffset();

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
            for (var j = 0; j < chunkTableSize.x ; j++)
            {
                Handles.Label(chunkOffset + i * downVector + j * rightVector + new Vector3(0.1f, -0.1f),
                    $"Chunk {counter++}");
            }
        }
    }
}