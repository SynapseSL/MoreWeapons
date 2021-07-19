using UnityEngine;

namespace MoreWeapons.Scripts
{
    public class ExplodeScript : MonoBehaviour
    {
        public GameObject owner;

        public void OnCollisionEnter(Collision col)
        {
            if (col.gameObject == owner || col.gameObject.GetComponent<Grenades.Grenade>() != null) return;
            GetComponent<Grenades.Grenade>().NetworkfuseTime = 0.10000000149011612;
        }
    }
}
