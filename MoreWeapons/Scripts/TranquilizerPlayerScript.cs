using System.Collections.Generic;
using System.Linq;
using MEC;
using Synapse.Api;
using Synapse.Api.Enum;
using UnityEngine;

namespace MoreWeapons.Scripts
{
    public class TranquilizerPlayerScript : MonoBehaviour
    {
        public TranquilizerPlayerScript() => Player = GetComponent<Player>();

        private readonly Player Player;

        public bool Stuned { get; private set; }

        public void Stun() => Timing.RunCoroutine(StunCoroutine());

        private IEnumerator<float> StunCoroutine()
        {
            if (PluginClass.TzConfig.BlockedIDs.Any(x => Player.RoleID == x)) yield break;

            Player.GiveTextHint("You are <color=blue>stuned</color> by a <color=blue>Tranquilizer</color>");

            Synapse.Api.Ragdoll rag = null;
            if (PluginClass.TzConfig.SpawnRagdoll)
                rag = Synapse.Api.Ragdoll.CreateRagdoll(Player.RoleType, Player.Position, Quaternion.identity, Vector3.zero, new PlayerStats.HitInfo(), false, Player);

            if (PluginClass.TzConfig.DropInventory)
                Player.Inventory.DropAll();

            var pos = Player.Position;
            Player.GodMode = true;
            Player.Position = Vector3.up;

            Stuned = true;

            yield return Timing.WaitForSeconds(Random.Range(PluginClass.TzConfig.MinStunTime, PluginClass.TzConfig.MaxStunTime));

            if (rag != null) rag.Destroy();
            Player.Position = pos;
            Stuned = false;
            Player.GodMode = false;

            if (Map.Get.Nuke.Detonated && Player.Zone != ZoneType.Surface)
            {
                yield return Timing.WaitForSeconds(0.1f);
                Player.Hurt(99999);
            }
        }
    }
}
