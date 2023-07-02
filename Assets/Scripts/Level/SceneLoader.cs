using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public void TryExitTheLocation(EntryExit exit)
    {
        var exitIndexes = Player.Instance.GetLastLevelData().exitIndexes;
        foreach (var exitIndex in exitIndexes)
        {
            if (GameBus.Instance.GetLevel().GetEntryExits()[exitIndex] == exit)
            {
                Chunks.Instance.ClearChunksData();
                Player.Instance.ClearPlayerData();
                SceneManager.LoadScene(0);
                return;
            }
        }
    }
}
