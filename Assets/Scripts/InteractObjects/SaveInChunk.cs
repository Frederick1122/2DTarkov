using UnityEditor;
using UnityEngine;

public class SaveInChunk<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T _saveObject;

    [SerializeField][HideInInspector] private int _index;

    public void SetIndex(int index)
    {
        _index = index;
        EditorUtility.SetDirty(this);
    }

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