using UnityEngine;

namespace InventorySystem
{
    public class ItemRegistery : MonoBehaviour
    {
        //singlenton
        public static ItemRegistery instance;
        //Items
        [SerializeField] BaseItemData[] Items;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void AddItem(string id, Inventory inventory)
        {
            BaseItemData ItemToAdd = null;

            foreach (var item in Items)
            {
                if (item.ItemID == id)
                {
                    ItemToAdd = item;
                    break;
                }
            }

            if (ItemToAdd == null) return;

            inventory.AddItem(ItemToAdd);
        }
    }
}