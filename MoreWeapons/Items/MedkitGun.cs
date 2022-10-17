using Neuron.Core.Meta;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;

namespace MoreWeapons.Items;

[Automatic]
[Item(
    Id = (uint)CustomItemType.MedkitGun,
    Name = nameof(CustomItemType.MedkitGun),
    BasedItemType = ItemType.GunFSP9
    )]
public class MedkitGun : MoreWeaponsItemHandler
{
    public MedkitGun(ItemEvents items, PlayerEvents player, MoreWeapons moreWeapons) : base(items, player, moreWeapons) { }
    public override bool VanillaReload => false;
    public override bool Reloadable => MoreWeapons.MedkitGunConfig.CanBeReloaded;
    public override uint AmmoType => (uint)ItemType.Medkit;
    public override int MagazineSize => MoreWeapons.MedkitGunConfig.MagazineSize;
    
    public override void OnShoot(ShootEvent ev)
    {
        if (!ev.Allow || ev.Item.Durability == 0) return;
        ev.Allow = false;
        ev.Player.PlayerInteract.OnInteract();
        ev.Item.Durability--;
        if (ev.Target == null) return;
        ev.Target.Heal(MoreWeapons.MedkitGunConfig.HealAmount);
        Hitmarker.SendHitmarker(ev.Player.Connection, 1f);
    }
}