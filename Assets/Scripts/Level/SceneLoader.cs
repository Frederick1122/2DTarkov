using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public void TryExitTheLocation(EntryExit exit)
    {
        var exitIndexes = Player.Instance.GetExitIndexes();
        foreach (var exitIndex in exitIndexes)
        {
            if (GameBus.Instance.GetLevel().GetEntryExits()[exitIndex] == exit)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
