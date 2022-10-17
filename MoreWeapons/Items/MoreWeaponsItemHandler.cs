using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;

namespace MoreWeapons.Items;

public abstract class MoreWeaponsItemHandler : CustomItemHandler
{
    protected MoreWeapons MoreWeapons { get; }

    protected MoreWeaponsItemHandler(ItemEvents items, PlayerEvents player, MoreWeapons moreWeapons) : base(items,
        player)
    {
        MoreWeapons = moreWeapons;
    }

    public override void OnPickup(PickupEvent ev)
    {
        ev.Player.SendHint(MoreWeapons.Translation.Get(ev.Player).PickedUp.Replace("%item%", Attribute.Name), 8);
    }

    public override void OnEquip(ChangeItemEvent ev)
    {
        ev.Player.SendHint(MoreWeapons.Translation.Get(ev.Player).Equipped.Replace("%item%", Attribute.Name), 8);
    }
}