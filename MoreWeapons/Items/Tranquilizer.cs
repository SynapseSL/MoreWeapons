using Neuron.Core.Meta;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;

namespace MoreWeapons.Items;

[Automatic]
[Item(
    Id = (uint)CustomItemType.Tranquilizer,
    Name = nameof(CustomItemType.Tranquilizer),
    BasedItemType = ItemType.GunCOM18
    )]
public class Tranquilizer : MoreWeaponsItemHandler
{
    public Tranquilizer(ItemEvents items, PlayerEvents player, MoreWeapons moreWeapons) : base(items, player, moreWeapons) { }
    public override bool VanillaReload => false;
    public override bool Reloadable => MoreWeapons.TzConfig.Reloadable;
    public override uint AmmoType => (uint)ItemType.Ammo9x19;
    public override int MagazineSize => MoreWeapons.TzConfig.MagazineSize;
    public override int NeededForOneShot => MoreWeapons.TzConfig.AmmoNeededForOneShoot;

    public override void OnShoot(ShootEvent ev)
    {
        if (!ev.Allow || ev.Item.Durability == 0) return;
        ev.Player.PlayerInteract.OnInteract();
        ev.Allow = false;
        ev.Item.Durability--;
        if (ev.Target == null) return;
        ev.Target.GetComponent<MoreWeaponsScript>().Stun();
        Hitmarker.SendHitmarker(ev.Player.Connection, 1f);
    }
}