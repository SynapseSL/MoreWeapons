using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using UnityEngine;

namespace MoreWeapons.Scripts
{
    public class ExplodeScript : MonoBehaviour
    {
        public GameObject owner;

        public void OnCollisionEnter(Collision col)
        {
            if (col.gameObject == owner || GetComponent<ItemPickupBase>() is ExplosionGrenade) return;
            var grenade = (GetComponent<ItemPickupBase>() as ExplosionGrenade);
            grenade.DestroySelf();
        }
    }
}
