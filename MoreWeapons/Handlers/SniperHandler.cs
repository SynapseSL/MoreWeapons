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
        }

        private void OnPickup(Synapse.Api.Events.SynapseEventArguments.PlayerPickUpItemEventArgs ev)
        {
            if (ev.Item.ID == (int)CustomItemType.Sniper)
                ev.Player.GiveTextHint("You have picked up a Sniper");
        }
    }
}
