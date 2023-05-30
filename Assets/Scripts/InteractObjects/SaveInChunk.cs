using UnityEditor;
using UnityEngine;

public class SaveInChunk<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T _saveObject;

    [SerializeField][HideInInspector] private int _index;

    #if UNITY_EDITOR
    public void SetIndex(int index)
    {
        _index = index;
        EditorUtility.SetDirty(this);
    }
    
    #endif

    public int GetIndex() => _index;

    private void OnValidate()
    {
        UpdateFields();
    }

    private void Start()
    {
        if (_saveObject == null)
            Debug.LogError("SaveObject not found");
    }

    private void UpdateFields()
    {
        if (_saveObject == null || _saveObject == default)
            _saveObject = GetComponent<T>();
    }
}