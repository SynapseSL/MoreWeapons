using Synapse;
using System.Linq;
using UnityEngine;
using Mirror;
using Synapse.Api;

namespace MoreWeapons.Handlers
{
    public class GrenadeLauncherHandler
    {
        public GrenadeLauncherHandler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                BasedItemType = ItemType.GunLogicer,
                ID = (int)CustomItemType.GrenadeLauncher,
                Name = "GrenadeLauncher"
            });

            Server.Get.Events.Player.PlayerShootEvent += Shoot;
            Server.Get.Events.Player.PlayerReloadEvent += OnReload;
            Server.Get.Events.Player.PlayerPickUpItemEvent += OnPickup;
        }

        private void OnPickup(Synapse.Api.Events.SynapseEventArguments.PlayerPickUpItemEventArgs ev)
        {
            if (ev.Item.ID == (int)CustomItemType.GrenadeLauncher)
                ev.Player.GiveTextHint("You have picked up a GrenadeLauncher");
        }

        private void OnReload(Synapse.Api.Events.SynapseEventArguments.PlayerReloadEventArgs ev)
        {
            if(ev.Item.ID == (int)CustomItemType.GrenadeLauncher)
            {
                ev.Allow = false;

                if (!PluginClass.GLConfig.CanBeReloaded)
                    return;

                ev.Allow = false;
                foreach (var grenade in ev.Player.Inventory.Items.Where(x => x.ID == (int)ItemType.GrenadeFrag))
                {
                    if (ev.Item.Durabillity >= PluginClass.GLConfig.MagazineSize)
                        return;

                    grenade.Destroy();
                    ev.Item.Durabillity++;
                }
            }
        }

        private void Shoot(Synapse.Api.Events.SynapseEventArguments.PlayerShootEventArgs ev)
        {
            if(ev.Weapon.ID == (int)CustomItemType.GrenadeLauncher)
            {
                ev.Allow = false;
                ev.Weapon.Durabillity--;

                var velocity = (ev.TargetPosition - ev.Player.Position) * PluginClass.GLConfig.ForceMultiplier;
                var pos = ev.Player.CameraReference.TransformPoint(
                    ev.Player.GrenadeManager.availableGrenades[0].grenadeInstance.GetComponent<Grenades.Grenade>().throwStartPositionOffset);

                var grenade = Map.Get.SpawnGrenade(pos, velocity, PluginClass.GLConfig.GrenadeFuseTime, Synapse.Api.Enum.GrenadeType.Grenade, ev.Player);

                if (PluginClass.GLConfig.ExplodeOnCollison)
                {
                    var script = grenade.gameObject.AddComponent<ExplodeScript>();
                    script.owner = ev.Player.gameObject;
                }
            }
        }

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
}
