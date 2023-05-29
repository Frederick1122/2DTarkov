using UnityEngine;

public class SaveInChunk : MonoBehaviour
{
    [SerializeField] private InteractLootBox _interactLootBox;

    private int _index;

    public void SetIndex(int index) => _index = index;

    public int GetIndex() => _index;
    
    private void UpdateFields()
    {
        if ((_interactLootBox == null || _interactLootBox == default))
            _interactLootBox = GetComponent<InteractLootBox>();
    }
}