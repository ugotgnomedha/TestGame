using UnityEngine;

    public abstract class BaseItemData : ScriptableObject
    {
        [SerializeField] private string itemID;
        public string ItemID => itemID;

        public string itemName;
        public GameObject itemPrefab;
        public GameObject DropPrefab;
}