using UnityEngine;

namespace Inventory
{
    public class PlayerPickUpDrop : MonoBehaviour
    {
        [SerializeField] private Transform objectGrabPoint;
        private GameObject pickedUpItem;
        private Rigidbody pickedUpItem_rb;
        [SerializeField] LayerMask layermasksToAvoid;
        [SerializeField] float lerpSpeed = 10;

        public void useEquippedItem()
        {
            if (pickedUpItem == null) { return; }
            pickedUpItem.GetComponent<Pickupable>().Use();
        }

        public void setItem(GameObject newItemToPickUp)
        {
            if (pickedUpItem != null) { return; }
            pickedUpItem = newItemToPickUp;
            pickedUpItem_rb = pickedUpItem.GetComponent<Rigidbody>();
            pickedUpItem_rb.useGravity = false;
            pickedUpItem_rb.isKinematic = true;
            pickedUpItem_rb.excludeLayers = layermasksToAvoid;
        }
        public void dropItem()
        {
            if (pickedUpItem == null) { return; }
            StopClipping(pickedUpItem, pickedUpItem_rb, transform);
            pickedUpItem = null;
            pickedUpItem_rb.useGravity = true;
            pickedUpItem_rb.isKinematic = false;
            pickedUpItem_rb.excludeLayers = 0;
            pickedUpItem_rb = null;
        }

        private void StopClipping(GameObject obj, Rigidbody rig, Transform player)
        {
            Collider col = obj.GetComponent<Collider>();
            Vector3 target = col.bounds.center; 
            Vector3 dir = target - player.position;
            float dist = dir.magnitude;

            if (dist <= 0.01f) return;

            RaycastHit hit;

            if (Physics.Raycast(player.position, dir.normalized, out hit, dist))
            {
                if (hit.collider.gameObject == obj) return;

                float minDistance = 0.35f;
                float newDistance = Mathf.Max(hit.distance - 0.05f, minDistance);

                obj.transform.position = player.position + dir.normalized * newDistance;
                player.transform.position = player.position - dir.normalized * (dist - newDistance);
            }
        }

        private void FixedUpdate()
        {
            if (pickedUpItem != null)
            {
                Vector3 newPos =Vector3.Lerp(pickedUpItem.transform.position, objectGrabPoint.position,Time.deltaTime * lerpSpeed);
                pickedUpItem_rb.MovePosition(newPos);
            }
        }
    }
}
