using UnityEngine;
namespace Usable
{
    public class NewItem : MonoBehaviour, IUsable
    {
        public void Use()
        {
            Debug.Log("Item Used");
        }
    }
}
