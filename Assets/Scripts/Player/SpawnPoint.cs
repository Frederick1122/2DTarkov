using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public void SpawnItem(Item item, int count)
    {
        var newItem = Instantiate(GameManager.Instance.GetBaseItem(), transform);
        newItem.Init(item, count);
        newItem.transform.position = transform.position + new Vector3(0, 0, 2);
        
        newItem.transform.parent = null;
    }
}
