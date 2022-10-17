using InventorySystem.Items.Pickups;
using UnityEngine;

namespace MoreWeapons
{
    public class ExplodeScript : MonoBehaviour
    {
        public GameObject owner;

        public void OnCollisionEnter(Collision col)
        {
            if (col.gameObject == owner) return;
            GetComponent<ItemPickupBase>().GetItem().Destroy();
        }
    }
}
