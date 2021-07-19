using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreWeapons.Scripts
{
    public class C4PlayerComponent : MonoBehaviour
    {
        public List<Grenades.Grenade> C4 { get; } = new List<Grenades.Grenade>();

        public bool ExplodeAll()
        {
            var flag = false;
            foreach (var grenade in C4.ToList())
            {
                C4.Remove(grenade);
                if (grenade == null) continue;
                grenade.NetworkfuseTime = 0.10000000149011612;
                flag = true;
            }
            return flag;
        }
    }
}
