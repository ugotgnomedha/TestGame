using UnityEngine;
using Usable;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Transform drop_Pos;
        [SerializeField] private Transform hand_Right;
        private GameObject equipedItem_Right;
        private BaseItemData equipedData_Right;

        public void UseEquipedItem()
        {
            IUsable usable = null;
            if (equipedItem_Right != null && equipedData_Right != null)
            {
                equipedItem_Right.TryGetComponent<IUsable>(out usable);
                if (usable != null)
                    usable.Use();
            }
        }

        public void AddItem(BaseItemData item)
        {
            GameObject itemToAdd = null;
            if (equipedItem_Right == null)
            {
                itemToAdd = Instantiate(item.itemPrefab, hand_Right, false);
                itemToAdd.transform.localPosition = Vector3.zero;
                equipedItem_Right = itemToAdd;
                equipedData_Right = item;
            }
        }

        public void DropItem()
        {
            GameObject itemToDrop = null;
            if (equipedItem_Right != null)
            {
                itemToDrop = Instantiate(equipedData_Right.DropPrefab, drop_Pos.transform.position,Quaternion.identity);
                Destroy(equipedItem_Right.gameObject);
                equipedItem_Right = null;
                equipedData_Right = null;
            }
        }
    }
}
