using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Version : MonoBehaviour
{
    private const string VERSION_DATA_PATH = "Version";

    [SerializeField] private TMP_Text _text;
    
    private void OnValidate()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        var versionData = Resources.Load<VersionData>(VERSION_DATA_PATH);
        _text.text = $"Ver. {versionData.GetVersion()}";
    }
}
