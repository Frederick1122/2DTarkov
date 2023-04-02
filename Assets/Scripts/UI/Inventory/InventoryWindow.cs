using Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryWindow : MonoBehaviour
    {
        [SerializeField] private InventoryWindowCell _inventoryCell;
        [SerializeField] private GridLayoutGroup _inventoryLayoutGroup;
        [SerializeField] private ItemInformationPanel _itemInformationPanel;
        public void Refresh()
        {
            _itemInformationPanel.SetNewInformation();
            
            var content = _inventoryLayoutGroup.gameObject;
            foreach (Transform child in content.transform) {
                Destroy(child.gameObject);
            }

            var inventory = InventoryManager.Instance.GetInventory();

            foreach (var cell in inventory.inventoryCells)
            {
                var newCell = Instantiate(_inventoryCell, _inventoryLayoutGroup.transform);
                newCell.Init(_itemInformationPanel, cell.GetItem(), cell.Count);
            }
        }
    }
}
