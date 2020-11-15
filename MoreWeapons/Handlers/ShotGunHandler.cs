using Synapse;

namespace MoreWeapons.Handlers
{
    public class ShotGunHandler
    {
        public ShotGunHandler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                ID = (int)CustomItemType.ShotGun,
                Name = "ShotGun",
                BasedItemType = ItemType.GunMP7,
            });

            Server.Get.Events.Player.PlayerShootEvent += OnShoot;
            Server.Get.Events.Player.PlayerPickUpItemEvent += OnPickup;
        }

        private void OnPickup(Synapse.Api.Events.SynapseEventArguments.PlayerPickUpItemEventArgs ev)
        {
            if(ev.Item.ID == (int)CustomItemType.ShotGun)
                ev.Player.GiveTextHint("You have picked up a ShotGun");
        }

        private void OnShoot(Synapse.Api.Events.SynapseEventArguments.PlayerShootEventArgs ev)
        {
            if(ev.Weapon.ID == (int)CustomItemType.ShotGun)
            {
                ev.Allow = false;
            }
        }
    }
}
