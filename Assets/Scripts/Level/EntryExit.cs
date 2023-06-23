using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EntryExit : MonoBehaviour
{
    [SerializeField] private List<EntryExit> _currentExits = new List<EntryExit>();

    public List<EntryExit> GetCurrentExits() => _currentExits;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerHumanoid>())
        {
            SceneLoader.Instance.TryExitTheLocation(this);
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