using Synapse;
using System.Linq;
using UnityEngine;
using Mirror;

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

                var component = ev.Player.GetComponent<Grenades.GrenadeManager>();
                var component2 = Object.Instantiate(component.availableGrenades[0].grenadeInstance).GetComponent<Grenades.Grenade>();
                var velocity = (ev.TargetPosition - ev.Player.Position) * PluginClass.GLConfig.ForceMultiplier;
                component2.FullInitData(component, ev.Player.CameraReference.TransformPoint(component2.throwStartPositionOffset), Quaternion.Euler(component2.throwStartAngle), velocity, component2.throwAngularVelocity,ev.Player.Team);
                component2.NetworkfuseTime = NetworkTime.time + (double)PluginClass.GLConfig.GrenadeFuseTime;
                NetworkServer.Spawn(component2.gameObject);
            }
        }
    }
}
