using Synapse;

namespace MoreWeapons.Handlers
{
    public class SniperHandler
    {
        public SniperHandler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                ID = (int)CustomItemType.Sniper,
                BasedItemType = ItemType.GunE11SR,
                Name = "Sniper"
            });

            Server.Get.Events.Player.PlayerPickUpItemEvent += OnPickup;
            Server.Get.Events.Player.PlayerDamageEvent += OnDamage;
            Server.Get.Events.Player.PlayerReloadEvent += Reload;
        }

        private void OnDamage(Synapse.Api.Events.SynapseEventArguments.PlayerDamageEventArgs ev)
        {
            if (ev.Killer == null) return;

            if (ev.Killer.ItemInHand == null) return;

            if (ev.Killer.ItemInHand.ID == (int)CustomItemType.Sniper && ev.HitInfo.GetDamageType() == DamageTypes.E11StandardRifle)
                ev.DamageAmount = ev.Victim.RoleType == RoleType.Scp106 ? PluginClass.SnConfig.Damage / 10f : PluginClass.SnConfig.Damage;
        }

        private void Reload(Synapse.Api.Events.SynapseEventArguments.PlayerReloadEventArgs ev)
        {
            if (ev.Item.ID == (int)CustomItemType.Sniper)
            {
                ev.Allow = false;

                if (ev.Item.Durabillity >= PluginClass.SnConfig.MagazineSize) return;

                var reloadAmount = PluginClass.SnConfig.MagazineSize - ev.Item.Durabillity;

                if (ev.Player.Ammo5 < reloadAmount * PluginClass.SnConfig.AmooNeededForOneShoot)
                    reloadAmount = ev.Player.Ammo5 / PluginClass.SnConfig.AmooNeededForOneShoot;

                ev.Item.Durabillity += reloadAmount;
                ev.Player.Ammo5 -= (uint)reloadAmount * (uint)PluginClass.SnConfig.AmooNeededForOneShoot;
            }
        }

        private void OnPickup(Synapse.Api.Events.SynapseEventArguments.PlayerPickUpItemEventArgs ev)
        {
            if (ev.Item.ID == (int)CustomItemType.Sniper)
                ev.Player.GiveTextHint("You have picked up a Sniper");
        }
    }
}
