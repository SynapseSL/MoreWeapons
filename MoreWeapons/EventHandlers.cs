using Synapse;
using System.Linq;

namespace MoreWeapons
{
    public class EventHandlers
    {
        public EventHandlers()
        {
            Server.Get.Events.Player.PlayerPickUpItemEvent += Pickup;
            Server.Get.Events.Player.PlayerChangeItemEvent += Equip;
        }

        private void Pickup(Synapse.Api.Events.SynapseEventArguments.PlayerPickUpItemEventArgs ev)
        {
            if (System.Enum.GetValues(typeof(CustomItemType)).ToArray<CustomItemType>().Any(x => (int)x == ev.Item.ID))
                ev.Player.GiveTextHint($"You have picked up a {(CustomItemType)ev.Item.ID}");
        }
        
         private void Equip(Synapse.Api.Events.SynapseEventArguments.PlayerChangeItemEventArgs ev)
        {
            if (System.Enum.GetValues(typeof(CustomItemType)).ToArray<CustomItemType>().Any(x => (int)x == ev.NewItem.ID))
                ev.Player.GiveTextHint($"You have equipped {(CustomItemType)ev.NewItem.ID}");
        }
    }
}
