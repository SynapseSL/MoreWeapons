using InventorySystem;
using InventorySystem.Items.ThrowableProjectiles;
using Neuron.Core.Meta;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;
using UnityEngine;

namespace MoreWeapons.Items;

[Automatic]
[Item(
    Id = (uint)CustomItemType.GrenadeLauncher,
    Name = nameof(CustomItemType.GrenadeLauncher),
    BasedItemType = ItemType.GunLogicer
    )]
public class GrenadeLauncher : MoreWeaponsItemHandler
{
    public GrenadeLauncher(ItemEvents items, PlayerEvents player, MoreWeapons plugin) : base(items, player,plugin) { }
    public override bool VanillaReload => false;
    public override bool Reloadable => MoreWeapons.GLConfig.CanBeReloaded;
    public override uint AmmoType => (uint)ItemType.GrenadeHE;
    public override int MagazineSize => MoreWeapons.GLConfig.MagazineSize;

    public override void OnShoot(ShootEvent ev)
    {
        if (!ev.Allow || ev.Item.Durability == 0) return;
        ev.Player.PlayerInteract.OnInteract();
        ev.Allow = false;
        ev.Item.Durability--;

        var defaultItem = InventoryItemLoader.AvailableItems[ItemType.GrenadeHE] as ThrowableItem;
        var settings = defaultItem.FullThrowSettings;
        var reference = ev.Player.CameraReference;
        var a2 = reference.forward + (reference.up * settings.UpwardsFactor) *
            (1f - Mathf.Abs(Vector3.Dot(reference.forward, Vector3.up)));
        var velocity = ev.Player.PlayerMovementSync.PlayerVelocity + a2 * 20 * MoreWeapons.GLConfig.ForceMultiplier;

        var grenade = new SynapseItem(ItemType.GrenadeHE, ev.Player.CameraReference.position);
        grenade.Pickup.Rb.velocity = velocity;
        grenade.Throwable.Fuse(ev.Player);
        grenade.Throwable.FuseTime = MoreWeapons.GLConfig.GrenadeFuseTime;

        if (MoreWeapons.GLConfig.ExplodeOnCollision)
        {
            var script = grenade.Throwable.Projectile.gameObject.AddComponent<ExplodeScript>();
            script.owner = ev.Player.gameObject;
        }
    }
}