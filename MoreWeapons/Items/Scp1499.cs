using InventorySystem.Items.Usables;
using Neuron.Core.Meta;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;
using UnityEngine;

namespace MoreWeapons.Items;

[Automatic]
[Item(
    Id = (uint)CustomItemType.Scp1499,
    Name = nameof(CustomItemType.Scp1499),
    BasedItemType = ItemType.SCP268
    )]
public class Scp1499 : MoreWeaponsItemHandler
{
    public Scp1499(ItemEvents items, PlayerEvents player, MoreWeapons moreWeapons) : base(items, player, moreWeapons) { }

    public override void OnDrop(DropItemEvent ev)
    {
        var script = ev.Player.GetComponent<MoreWeaponsScript>();
        if (script.In1499Dimension)
            script.Leave1499();
    }

    public override void OnConsume(ConsumeItemEvent ev)
    {
        switch (ev.State)
        {
            case ItemInteractState.Start:
                if (!ev.Item.ObjectData.ContainsKey("1499")) return;
                if (ev.Item.ObjectData["1499"] is not float time) return;

                ev.Allow = Time.time >= time;
                if (!ev.Allow)
                    ev.Player.SendNetworkMessage(new ItemCooldownMessage(ev.Item.Serial, time - Time.time));
                break;
            
            case ItemInteractState.Finalize:
                ev.Allow = false;
                var script = ev.Player.GetComponent<MoreWeaponsScript>();
                if (script.In1499Dimension)
                {
                    script.Leave1499();
                }
                else
                {
                    script.Enter1499(ev.Item);
                }
                break;
        }
    }
}