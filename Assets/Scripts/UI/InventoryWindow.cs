using Base;
using UnityEngine;

public class InventoryWindow : MonoBehaviour
{
    private void Refresh()
    {
        var inventory = InventoryManager.GetInstance().GetInventory();
    }
}
