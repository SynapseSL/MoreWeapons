using Synapse;
using System.Linq;

namespace MoreWeapons.Handlers
{
    public class MedkitGunHandler
    {
        public MedkitGunHandler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                BasedItemType = ItemType.GunMP7,
                ID = (int)CustomItemType.MedkitGun,
                Name = "MedkitGun"
            });

            Server.Get.Events.Player.PlayerShootEvent += Shoot;
            Server.Get.Events.Player.PlayerReloadEvent += Reload;
        }

        private void Reload(Synapse.Api.Events.SynapseEventArguments.PlayerReloadEventArgs ev)
        {
            if (ev.Item?.ID == (int)CustomItemType.MedkitGun)
            {
                ev.Allow = false;

                if (!PluginClass.MedkitGunConfig.CanBeReloaded)
                    return;

                foreach (var medkit in ev.Player.Inventory.Items.Where(x => x.ID == (int)ItemType.Medkit))
                {
                    if (ev.Item.Durabillity >= PluginClass.MedkitGunConfig.MagazineSize)
                        return;

                    medkit.Destroy();
                    ev.Item.Durabillity++;
                }
            }
        }

        private void Shoot(Synapse.Api.Events.SynapseEventArguments.PlayerShootEventArgs ev)
        {
            if (ev.Weapon?.ID == (int)CustomItemType.MedkitGun)
            {
                ev.Player.PlayerInteract.OnInteract();
                ev.Allow = false;
                ev.Weapon.Durabillity--;

                if(ev.Target != null)
                {
                    ev.Target.Heal(PluginClass.MedkitGunConfig.HealAmount);
                    ev.Player.WeaponManager.RpcConfirmShot(true, ev.Player.WeaponManager.curWeapon);
                }
                else ev.Player.WeaponManager.RpcConfirmShot(false, ev.Player.WeaponManager.curWeapon);
            }
        }
    }
}
