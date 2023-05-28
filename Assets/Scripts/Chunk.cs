using System;
using System.Collections.Generic;
using InteractObjects;
using UnityEngine;

[Serializable]
public class Chunk
{
    [SerializeField] private int _chunkIndex;
    [SerializeField] private List<SaveInChunk> _chunkObjects = new List<SaveInChunk>();

    public void Init(int chunkIndex, List<SaveInChunk> chunkObjects)
    {
        _chunkIndex = chunkIndex;
        SetChunkObjects(chunkObjects);
    }
    
    private void SetChunkObjects(List<SaveInChunk> chunkObjects)
    {
        _chunkObjects.Clear();
        
        for (var i = 0; i < chunkObjects.Count; i++)
        {
            var index = _chunkIndex * 100000 + 1 * 10000 + i;
            chunkObjects[i].SetIndex(index);
        }

        _chunkObjects.AddRange(chunkObjects);
    }
}