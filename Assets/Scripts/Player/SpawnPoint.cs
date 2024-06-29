using System.Collections;
using System.Collections.Generic;
using ConfigScripts;
using InteractObjects;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private InteractItem _baseItem;
    public void SpawnItem(ItemConfig itemConfig, int count)
    {   
        var newItem = Instantiate(_baseItem, transform);
        newItem.Init(itemConfig, count, transform.position + new Vector3(0, 0, 2), itemConfig.dropIcon);

        newItem.transform.parent = null;
    }
}
