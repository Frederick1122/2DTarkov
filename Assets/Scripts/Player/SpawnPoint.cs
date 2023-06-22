using System.Collections;
using System.Collections.Generic;
using InteractObjects;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private InteractItem _baseItem;
    public void SpawnItem(Item item, int count)
    {
        if (_baseItem == null)
        {
            _baseItem = GameBus.Instance.GetBaseItem();
        }
        
        var newItem = Instantiate(_baseItem, transform);
        newItem.Init(item, count, transform.position + new Vector3(0, 0, 2), item.dropIcon);

        newItem.transform.parent = null;
    }
}
