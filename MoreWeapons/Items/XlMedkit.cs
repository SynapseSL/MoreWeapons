using Neuron.Core.Meta;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;

namespace MoreWeapons.Items;

[Automatic]
[Item(
    Id = (uint)CustomItemType.XlMedkit,
    Name = nameof(CustomItemType.XlMedkit),
    BasedItemType = ItemType.Medkit
    )]
public class XlMedkit : MoreWeaponsItemHandler
{
    public XlMedkit(ItemEvents items, PlayerEvents player, MoreWeapons moreWeapons) : base(items, player, moreWeapons) { }

    public override void OnConsume(ConsumeItemEvent ev)
    {
        if(!ev.Allow || ev.State != ItemInteractState.Finalize) return;
        
        ev.Allow = false;
        if (!ev.Item.ObjectData.ContainsKey("xlmedkit"))
            ev.Item.ObjectData["xlmedkit"] = MoreWeapons.XlMedkitConfig.AmountOfUsages;
        if (ev.Item.ObjectData["xlmedkit"] is not (int usages and > 0)) return;

        var newUsages = usages - 1;

        if (newUsages <= 0)
        {
            ev.Item.Destroy();
        }
        else
        {
            ev.Player.SendHint(MoreWeapons.Translation.Get(ev.Player).XLMedkitUSages
                .Replace("%amount%", newUsages.ToString()));
            ev.Item.ObjectData["xlmedkit"] = newUsages;
        }
        ev.Player.Heal(MoreWeapons.XlMedkitConfig.HealAmount);
    }
}