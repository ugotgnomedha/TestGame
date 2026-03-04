using UnityEngine;

namespace Inventory
{
    public class PlayerPickUpDrop : MonoBehaviour
    {
        [SerializeField] private Transform objectGrabPoint;
        private GameObject pickedUpItem;
        private Rigidbody pickedUpItem_rb;
        [SerializeField] LayerMask layermasksToAvoid;
        [SerializeField] float LerpSpeed = 10;

        public void setItem(GameObject newItemToPickUp)
        {
            if (pickedUpItem == null)
            {
                pickedUpItem = newItemToPickUp;
                pickedUpItem_rb = pickedUpItem.GetComponent<Rigidbody>();
                pickedUpItem_rb.useGravity = false;
                pickedUpItem_rb.isKinematic = true;
                pickedUpItem_rb.excludeLayers = layermasksToAvoid;
            }
        }

        private void FixedUpdate()
        {
            if (pickedUpItem != null)
            {
                Vector3 newPos =Vector3.Lerp(pickedUpItem.transform.position, objectGrabPoint.position,Time.deltaTime * LerpSpeed);
                pickedUpItem_rb.MovePosition(newPos);
            }
        }
    }
}
