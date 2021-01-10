using Synapse;

namespace MoreWeapons.Handlers
{
    public class XlMedkitHandler
    {
        public XlMedkitHandler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                ID = (int)CustomItemType.XlMedkit,
                Name = "XlMedkit",
                BasedItemType = ItemType.Medkit
            });

            Server.Get.Events.Player.PlayerPickUpItemEvent += OnPickup;
            Server.Get.Events.Player.PlayerItemUseEvent += OnItemUse;
        }

        private void OnItemUse(Synapse.Api.Events.SynapseEventArguments.PlayerItemInteractEventArgs ev)
        {
            if(ev.CurrentItem?.ID == (int)CustomItemType.XlMedkit && ev.State == Synapse.Api.Events.SynapseEventArguments.ItemInteractState.Finalizing)
                ev.Player.Inventory.AddItem(ItemType.Medkit, 0, 0, 0, 0);
        }

        private void OnPickup(Synapse.Api.Events.SynapseEventArguments.PlayerPickUpItemEventArgs ev)
        {
            if(ev.Item?.ID == (int)CustomItemType.XlMedkit)
                ev.Player.GiveTextHint("You have picked up a XlMedkit");
        }
    }
}
