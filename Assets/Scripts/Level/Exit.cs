using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private List<Exit> _currentExits = new List<Exit>();

    public List<Exit> GetCurrentExits() => _currentExits;
}


#if UNITY_EDITOR

[CustomEditor(typeof(Exit))]
public class ExitEditor : Editor
{
    public void OnSceneGUI()
    {
        var exit = target as Exit;

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