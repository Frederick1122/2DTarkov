using System.Collections.Generic;
using Managers.SaveLoadManagers;
using UI;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EntryExit : MonoBehaviour
{
    private const string TEXT_ON_EXIT = "You survived this time";
    
    [SerializeField] private string _name;
    [Space]
    [SerializeField] private List<EntryExit> _currentExits = new List<EntryExit>();

    public List<EntryExit> GetCurrentExits()
    {
        return _currentExits;
    }

    public string GetName()
    {
        return _name;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerHumanoid>())
        {
            var exitIndexes = PlayerSaveLoadManager.Instance.GetLastLevelData().exitIndexes;
            foreach (var exitIndex in exitIndexes)
            {
                if (GameBus.Instance.Level.GetEntryExits()[exitIndex] == this)
                {
                    ChunksSaveLoadManager.Instance.ClearChunksData();
                    PlayerSaveLoadManager.Instance.ClearPlayerData();
                    //TODO: repair endGameUI
                    //UIManager.Instance.OpenEndGameUI(TEXT_ON_EXIT);
                    UIManager.Instance.OpenWindow<EndGameWindowController>();
                    return;
                }
            }
        }
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(EntryExit))]
public class ExitEditor : Editor
{
    public void OnSceneGUI()
    {
        var exit = target as EntryExit;

        if (exit == null)
            return;
        
        foreach (var currentExit in exit.GetCurrentExits())
        {
            Handles.DrawLine(exit.transform.position, currentExit.transform.position);
            Handles.Label(currentExit.transform.position, currentExit.name);
        }
    }
}

#endif