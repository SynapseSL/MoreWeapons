using System.Collections.Generic;
using System.Linq;
using MEC;
using Neuron.Core.Meta;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;

namespace MoreWeapons.Items;

[Automatic]
[Item(
    Id = (uint)CustomItemType.Scp127,
    Name = nameof(CustomItemType.Scp127),
    BasedItemType = ItemType.GunFSP9
    )]
public class Scp127 : MoreWeaponsItemHandler
{
    private readonly RoundEvents _roundEvents;
    private readonly ItemService _item;
    private CoroutineHandle _coroutineHandle;

    public Scp127(ItemEvents items, PlayerEvents player, MoreWeapons moreWeapons, RoundEvents roundEvents,ItemService item) : base(items,
        player, moreWeapons)
    {
        _roundEvents = roundEvents;
        _item = item;
    }

    public override void OnReload(ReloadWeaponEvent ev) => ev.Allow = false;
    public override void HookEvents()
    {
        base.HookEvents();
        _roundEvents.Waiting.Subscribe(Waiting);
        _roundEvents.Restart.Subscribe(Restart);
    }

    private void Waiting(RoundWaitingEvent _)
    {
        if (_coroutineHandle.IsRunning) Timing.KillCoroutines(_coroutineHandle);
        _coroutineHandle = Timing.RunCoroutine(Refill());
    }

    private void Restart(RoundRestartEvent _)
    {
        if (_coroutineHandle.IsRunning) Timing.KillCoroutines(_coroutineHandle);
    }

    private IEnumerator<float> Refill()
    {
        for (; ; )
        {
            foreach (var item in _item.AllItems.Where(x => x.Id == (uint)CustomItemType.Scp127))
            {
                if (item.Durability >= MoreWeapons.Scp127Config.MaxAmmo) continue;
                var durability = item.Durability + MoreWeapons.Scp127Config.ReloadAmount;
                if (durability >= MoreWeapons.Scp127Config.MaxAmmo) item.Durability = MoreWeapons.Scp127Config.MaxAmmo;
                else item.Durability = durability;
            }
            yield return Timing.WaitForSeconds(MoreWeapons.Scp127Config.ReloadInterval);
        }
    }
}