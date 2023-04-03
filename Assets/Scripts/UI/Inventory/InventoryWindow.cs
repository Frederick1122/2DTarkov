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
        
        [Space] [Header("Action Buttons")]
        [SerializeField] private Button _useButton;
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _divideButton;
        [SerializeField] private Button _dropButton;

        private InventoryWindowCell _currentCell;
        
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
                var cellItem = cell.GetItem(); 
                newCell.Init(cellItem, cell.Count);
                newCell.GetButton().onClick.AddListener(() =>
                {
                    
                    
                });
                
            }
        }

        private void ClickOnCell(InventoryWindowCell cell)
        {
            var cellItem = cell.GetItem(); 
            _currentCell = _currentCell == cell ? null : cell;
            if (_currentCell != null) 
                _itemInformationPanel.SetNewInformation(cellItem.icon, cellItem.name, cellItem.description);

            RefreshActionButtons(cellItem);
        }
        
        private void RefreshActionButtons(Item item)
        {
           //TODO
        }
        
    }
}
