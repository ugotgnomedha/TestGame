using UnityEngine;
using Usable;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Transform hand;
        [SerializeField] private Transform dropPos;
        private GameObject equipedItem;
        private BaseItemData equipedData;
        private bool isEquiped;

        private void Update()
        {
            isEquiped = (equipedItem != null && equipedData != null);
        }

        public void UseEquipedItem()
        {
            IUsable usable = null;
            if (isEquiped)
            {
                equipedItem.TryGetComponent<IUsable>(out usable);
                if (usable != null)
                    usable.Use();
            }
        }

        public void AddItem(BaseItemData item)
        {
            GameObject itemToAdd = null;
            if (!isEquiped)
            {
                itemToAdd = Instantiate(item.itemPrefab, hand, false);
                itemToAdd.transform.localPosition = Vector3.zero;
                equipedItem = itemToAdd;
                equipedData = item;
            }
        }

        public void DropItem()
        {
            GameObject itemToDrop = null;
            if (isEquiped)
            {
                itemToDrop = Instantiate(equipedData.DropPrefab, dropPos.transform.position,Quaternion.identity);
                Destroy(equipedItem.gameObject);
                equipedItem = null;
                equipedData = null;
            }
        }
    }
}
