using Neuron.Core.Meta;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;
using UnityEngine;

namespace MoreWeapons.Items;

[Automatic]
[Item(
    Id = (uint)CustomItemType.Sniper,
    Name = nameof(CustomItemType.Sniper),
    BasedItemType = ItemType.GunE11SR
)]
public class Sniper : MoreWeaponsItemHandler
{
    public Sniper(ItemEvents items, PlayerEvents player, MoreWeapons moreWeapons) : base(items, player, moreWeapons) { }
    public override bool VanillaReload => false;
    public override uint AmmoType => (uint)ItemType.Ammo556x45;
    public override int MagazineSize => MoreWeapons.SnConfig.MagazineSize;
    public override int NeededForOneShot => MoreWeapons.SnConfig.AmmoNeededForOneShoot;

    public override void OnShoot(ShootEvent ev)
    {
        if (!ev.Allow || ev.Item.Durability == 0) return;
        ev.Allow = false;
        if (ev.Item.ObjectData.ContainsKey("sniper") && ev.Item.ObjectData["sniper"] is float time)
        {
            if (Time.time < time)
            {
                ev.Player.SendHint(MoreWeapons.Translation.Get(ev.Player).SniperDelay
                    .Replace("%time%", (time - Time.time).ToString("00")));
                return;
            }
        }
        ev.Player.PlayerInteract.OnInteract();
        ev.Item.Durability--;
        ev.Item.ObjectData["sniper"] = Time.time + MoreWeapons.SnConfig.DelayBetweenShots;
        if (ev.Target == null) return;
        if (ev.Target.Hurt((int)(ev.Target.RoleType == RoleType.Scp106
                ? MoreWeapons.SnConfig.Damage / 10f
                : MoreWeapons.SnConfig.Damage), DamageType.Firearm))
            Hitmarker.SendHitmarker(ev.Player.Connection, 1f);
    }
}