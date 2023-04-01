using System;
using UnityEngine;

[Serializable]
public class Item : ScriptableObject
{
    protected string _directory = "";
    
    public string itemName;
    public string description;
    public float weight = 1f;
    public int maxStack = 1;
    public string configPath = "";

    internal virtual void OnValidate()
    {
        configPath = $"Prototypes/{_directory}" + this.name;
    }
}
