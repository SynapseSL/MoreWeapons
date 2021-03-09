using Synapse;
using Synapse.Api;
using Synapse.Api.Items;

namespace MoreWeapons.Handlers
{
    /*
    public class VaccinePistoleHandler
    {
        public VaccinePistoleHandler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                BasedItemType = ItemType.GunCOM15,
                ID = (int)CustomItemType.VaccinePistole,
                Name = "VaccinePistole"
            });

            Server.Get.Events.Player.PlayerShootEvent += Shoot;
        }

        private void Shoot(Synapse.Api.Events.SynapseEventArguments.PlayerShootEventArgs ev)
        {
            if (ev.Weapon?.ID == (int)CustomItemType.VaccinePistole)
            {
                ev.Player.PlayerInteract.OnInteract();
                ev.Allow = false;
                ev.Weapon.Durabillity--;

                if (ev.Target != null)
                {
                    ev.Player.WeaponManager.RpcConfirmShot(true, ev.Player.WeaponManager.curWeapon);
                    if (ev.Target.RoleID == (int)RoleType.Scp0492)
                    {
                        var pos = ev.Target.Position;
                        ev.Target.RoleType = RoleType.ClassD;
                        ev.Target.Position = pos;
                    }
                    else ev.Target.Hurt(10);
                }
                else ev.Player.WeaponManager.RpcConfirmShot(false, ev.Player.WeaponManager.curWeapon);
            }
        }
    
    }
    */
}
