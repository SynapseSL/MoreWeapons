using Neuron.Core.Meta;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;

namespace MoreWeapons.Items;

[Automatic]
[Item(
    Id = (uint)CustomItemType.VaccinePistol,
    Name = nameof(CustomItemType.VaccinePistol),
    BasedItemType = ItemType.GunCOM15
    )]
public class VaccinePistol : MoreWeaponsItemHandler
{
    public VaccinePistol(ItemEvents items, PlayerEvents player, MoreWeapons moreWeapons) : base(items, player, moreWeapons) { }
    public override bool VanillaReload => false;
    public override bool Reloadable => MoreWeapons.VPConfig.Reloadable;
    public override uint AmmoType => (uint)MoreWeapons.VPConfig.UsedAmmo;
    public override int MagazineSize => MoreWeapons.VPConfig.MagazineSize;

    public override void OnShoot(ShootEvent ev)
    {
        if (!ev.Allow || ev.Item.Durability == 0) return;
        ev.Allow = false;
        ev.Player.PlayerInteract.OnInteract();
        ev.Item.Durability--;
        if (ev.Target == null) return;
        if (ev.Player.RoleID == (uint)RoleType.Scp0492)
        {
            ev.Player.ChangeRoleLite(MoreWeapons.VPConfig.ReplaceRoles[
                UnityEngine.Random.Range(0, MoreWeapons.VPConfig.ReplaceRoles.Count)]);
            ev.Player.Health = 50f;
        }
        else
        {
            ev.Target.Heal(MoreWeapons.VPConfig.Heal);   
        }
        Hitmarker.SendHitmarker(ev.Player.Connection, 1f);
    }
}