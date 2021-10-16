using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using InventorySystem.Items.Firearms.BasicMessages;
using InventorySystem.Items.ThrowableProjectiles;
using MEC;
using Mirror;
using MoreWeapons.Scripts;
using Synapse;
using Synapse.Api;
using Synapse.Api.Enum;
using Synapse.Api.Events.SynapseEventArguments;
using Synapse.Api.Items;
using Synapse.Config;
using UnityEngine;

namespace MoreWeapons
{
    public class EventHandlers
    {
        public CustomItemType[] AllTypes { get; private set; }

        public EventHandlers()
        {
            Server.Get.Events.Player.PlayerChangeItemEvent += OnEquip;
            Server.Get.Events.Player.LoadComponentsEvent += OnLoadComponents;
            Server.Get.Events.Player.PlayerDamageEvent += OnDamage;
            Server.Get.Events.Player.PlayerDropItemEvent += OnDrop;
            Server.Get.Events.Player.PlayerItemUseEvent += OnItemUse;
            Server.Get.Events.Player.PlayerPickUpItemEvent += OnPickup;
            Server.Get.Events.Player.PlayerReloadEvent += OnReload;
            Server.Get.Events.Player.PlayerSetClassEvent += OnSetClass;
            Server.Get.Events.Player.PlayerShootEvent += OnShoot;
            Server.Get.Events.Round.RoundRestartEvent += OnRestart;
            Server.Get.Events.Round.RoundStartEvent += OnStart;
            Server.Get.Events.Round.WaitingForPlayersEvent += OnWaiting;

            AllTypes = (CustomItemType[])typeof(CustomItemType).GetEnumValues();
        }

        private void OnPickup(PlayerPickUpItemEventArgs ev)
        {
            if (AllTypes.Any(x => (int)x == ev.Item.ID))
                ev.Player.GiveTextHint($"You have picked up a {(CustomItemType)ev.Item.ID}");
        }

        private void OnEquip(Synapse.Api.Events.SynapseEventArguments.PlayerChangeItemEventArgs ev)
        {
            if (AllTypes.Any(x => (int)x == ev.NewItem.ID))
                ev.Player.GiveTextHint($"You have equipped {(CustomItemType)ev.NewItem.ID}");
        }

        //Used by:
        //Tranquilizer
        //XLMedkit
        //SCP-1499
        private void OnItemUse(PlayerItemInteractEventArgs ev)
        {
            if (ev.Player.GetComponent<TranquilizerPlayerScript>().Stuned)
            {
                ev.Allow = false;
                return;
            }

            switch (ev.CurrentItem?.ID)
            {
                case (int)CustomItemType.XlMedkit when ev.State == Synapse.Api.Events.SynapseEventArguments.ItemInteractState.Finalizing:
                    ev.Player.Inventory.AddItem(ItemType.Medkit);
                    break;

                case (int)CustomItemType.Scp1499 when ev.State == Synapse.Api.Events.SynapseEventArguments.ItemInteractState.Finalizing:
                    ev.Player.GetComponent<Scp1499PlayerScript>().Use1499();
                    ev.Allow = false;
                    break;
            }
        }

        //Used by:
        //Tranquilizer
        //SCP-1499
        private void OnDrop(PlayerDropItemEventArgs ev)
        {
            if (ev.Player.GetComponent<TranquilizerPlayerScript>().Stuned)
            {
                ev.Allow = false;
                return;
            }

            switch (ev.Item?.ID)
            {
                case (int)CustomItemType.Scp1499 when ev.Player.GetComponent<Scp1499PlayerScript>().IsInDimension:
                    ev.Allow = false;
                    ev.Player.GiveTextHint("You can't currently drop Scp1499");
                    break;
            }
        }

        //Used by:
        //Tranquilizer
        //GrenadLauncher
        //MedkitGun
        //VaccinePistole
        private void OnShoot(PlayerShootEventArgs ev)
        {
            if (ev.Player.GetComponent<TranquilizerPlayerScript>().Stuned)
            {
                ev.Allow = false;
                return;
            }

            switch (ev.Weapon?.ID)
            {
                case (int)CustomItemType.Tranquilizer:

                    ev.Player.PlayerInteract.OnInteract();
                    ev.Allow = false;
                    ev.Weapon.Durabillity--;

                    if (ev.Target != null)
                    {
                        ev.Target.GetComponent<TranquilizerPlayerScript>().Stun();
                        Hitmarker.SendHitmarker(ev.Player.Connection, 1f);
                    }
                    break;

                case (int)CustomItemType.MedkitGun:
                    ev.Player.PlayerInteract.OnInteract();
                    ev.Allow = false;
                    ev.Weapon.Durabillity--;

                    if (ev.Target != null)
                    {
                        ev.Target.Heal(PluginClass.MedkitGunConfig.HealAmount);
                        Hitmarker.SendHitmarker(ev.Player.Connection, 1f);
                    }
                    break;

                case (int)CustomItemType.GrenadeLauncher:
                    ev.Player.PlayerInteract.OnInteract();
                    ev.Allow = false;
                    ev.Weapon.Durabillity--;

                    var defaultItem = InventoryItemLoader.AvailableItems[ItemType.GrenadeHE] as ThrowableItem;
                    var settings = defaultItem.FullThrowSettings;
                    var reference = ev.Player.CameraReference;
                    var a2 = reference.forward + (reference.up * settings.UpwardsFactor) *
                        (1f - Mathf.Abs(Vector3.Dot(reference.forward, Vector3.up)));
                    var velocity = ev.Player.PlayerMovementSync.PlayerVelocity + a2 * 20 * PluginClass.GLConfig.ForceMultiplier;

                    var grenade = Map.Get.SpawnGrenade(ev.Player.CameraReference.position, velocity, 3, GrenadeType.Grenade, ev.Player);

                    if (PluginClass.GLConfig.ExplodeOnCollison)
                    {
                        var script = grenade.Throwable.ThrowableItem.gameObject.AddComponent<ExplodeScript>();
                        script.owner = ev.Player.gameObject;
                    }
                    break;

                case (int)CustomItemType.VaccinePistole:
                    ev.Player.PlayerInteract.OnInteract();
                    ev.Allow = false;
                    ev.Weapon.Durabillity--;

                    if (ev.Target != null)
                    {
                        Hitmarker.SendHitmarker(ev.Player.Connection, 1f);

                        if (ev.Target.RoleID == (int)RoleType.Scp0492)
                        {
                            var pos = ev.Target.Position;
                            ev.Target.RoleID = PluginClass.VPConfig.ReplaceRoles.ElementAt(UnityEngine.Random.Range(0, PluginClass.VPConfig.ReplaceRoles.Count));
                            ev.Target.Position = pos;
                        }
                        else ev.Target.Hurt(PluginClass.VPConfig.Damage);
                    }
                    break;
            }
        }

        //Used by:
        //Tranquilizer
        //MedkitGun
        //SCP-127
        //Sniper
        //GrenadeLauncher
        //VaccinePistole
        private void OnReload(PlayerReloadEventArgs ev)
        {
            switch (ev.Item?.ID)
            {
                case (int)CustomItemType.Tranquilizer:
                    ev.Allow = false;

                    if (ev.Item.Durabillity >= PluginClass.TzConfig.MagazineSize || !PluginClass.TzConfig.Reloadable) return;

                    var reloadAmount = PluginClass.TzConfig.MagazineSize - ev.Item.Durabillity;

                    if (ev.Player.AmmoBox[AmmoType.Ammo9x19] < reloadAmount * PluginClass.TzConfig.AmooNeededForOneShoot)
                        reloadAmount = ev.Player.AmmoBox[AmmoType.Ammo9x19] / PluginClass.TzConfig.AmooNeededForOneShoot;

                    ev.Item.Durabillity += reloadAmount;
                    ev.Player.AmmoBox[AmmoType.Ammo9x19] -= (ushort)(reloadAmount * PluginClass.TzConfig.AmooNeededForOneShoot);
                    break;

                case (int)CustomItemType.MedkitGun:
                    ev.Allow = false;

                    if (!PluginClass.MedkitGunConfig.CanBeReloaded)
                        return;

                    foreach (var medkit in ev.Player.Inventory.Items.Where(x => x.ID == (int)ItemType.Medkit))
                    {
                        if (ev.Item.Durabillity >= PluginClass.MedkitGunConfig.MagazineSize)
                            return;

                        medkit.Destroy();
                        ev.Item.Durabillity++;
                    }
                    break;

                case (int)CustomItemType.Scp127:
                    ev.Allow = false;
                    break;

                case (int)CustomItemType.Sniper:
                    ev.Allow = false;

                    if (ev.Item.Durabillity >= PluginClass.SnConfig.MagazineSize) return;

                    reloadAmount = PluginClass.SnConfig.MagazineSize - ev.Item.Durabillity;

                    if (ev.Player.AmmoBox[AmmoType.Ammo556x45] < reloadAmount * PluginClass.SnConfig.AmooNeededForOneShoot)
                        reloadAmount = ev.Player.AmmoBox[AmmoType.Ammo556x45] / PluginClass.SnConfig.AmooNeededForOneShoot;

                    ev.Item.Durabillity += reloadAmount;
                    ev.Player.AmmoBox[AmmoType.Ammo556x45] -= (ushort)(reloadAmount * PluginClass.SnConfig.AmooNeededForOneShoot);
                    break;

                case (int)CustomItemType.GrenadeLauncher:
                    ev.Allow = false;

                    if (!PluginClass.GLConfig.CanBeReloaded)
                        return;

                    foreach (var grenade in ev.Player.Inventory.Items.Where(x => x.ID == (int)ItemType.GrenadeHE))
                    {
                        if (ev.Item.Durabillity >= PluginClass.GLConfig.MagazineSize)
                            return;

                        grenade.Destroy();
                        ev.Item.Durabillity++;
                    }
                    break;

                case (int)CustomItemType.VaccinePistole:
                    ev.Allow = false;

                    foreach (var scp500 in ev.Player.Inventory.Items.Where(x => x.ID == (int)ItemType.SCP500))
                    {
                        if (ev.Item.Durabillity >= PluginClass.VPConfig.MagazineSize)
                            return;

                        scp500.Destroy();
                        ev.Item.Durabillity++;
                    }
                    break;
            }
        }

        //Used by:
        //Tranquilizer
        //SCP-1499
        private void OnLoadComponents(LoadComponentEventArgs ev)
        {
            if (ev.Player.GetComponent<TranquilizerPlayerScript>() == null)
                ev.Player.AddComponent<TranquilizerPlayerScript>();

            if (ev.Player.GetComponent<Scp1499PlayerScript>() == null)
                ev.Player.AddComponent<Scp1499PlayerScript>();
        }

        //Used by:
        //SCP-127
        private void OnStart() => scp127coroutine = Timing.RunCoroutine(Refill());

        //Used by:
        //SCP-127
        private void OnRestart() => Timing.KillCoroutines(scp127coroutine);

        //Used by:
        //Sniper
        private void OnDamage(PlayerDamageEventArgs ev)
        {
            if (ev.Killer == null || ev.Killer.ItemInHand == null || ev.Victim == null) return;

            switch (ev.Killer.ItemInHand.ID)
            {
                case (int)CustomItemType.Sniper when ev.HitInfo.Tool == DamageTypes.E11SR:
                    ev.DamageAmount = ev.Victim.RoleType == RoleType.Scp106 ? PluginClass.SnConfig.Damage / 10f : PluginClass.SnConfig.Damage;
                    break;
            }
        }

        //Used by:
        //SCP-1499
        private void OnWaiting()
        {
            if (PluginClass.Scp1499Config.SpawnDoor)
            {
                var door = Synapse.Api.Door.SpawnDoorVariant(doorSpawn.Parse().Position, Synapse.Api.Map.Get.GetRoom(MapGeneration.RoomName.Hcz049).GameObject.transform.rotation);
                door.GameObject.GetComponent<Interactables.Interobjects.DoorUtils.DoorVariant>().ServerChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason.SpecialDoorFeature, true);
            }
        }

        //Used by:
        //SCP-1499
        private void OnSetClass(Synapse.Api.Events.SynapseEventArguments.PlayerSetClassEventArgs ev)
        {
            ev.Player.GetComponent<Scp1499PlayerScript>().IsInDimension = false;
        }


        private readonly SerializedMapPoint doorSpawn = new SerializedMapPoint("HCZ_049", -6.683522f, 264.0f, 24.09575f);

        private MEC.CoroutineHandle scp127coroutine;

        private IEnumerator<float> Refill()
        {
            for (; ; )
            {
                foreach (var item in Map.Get.Items.Where(x => x.ID == (int)CustomItemType.Scp127))
                {
                    if (item.Durabillity >= PluginClass.Scp127Config.MaxAmmo) continue;

                    var newdur = item.Durabillity + PluginClass.Scp127Config.ReloadAmount;

                    if (newdur >= PluginClass.Scp127Config.MaxAmmo) item.Durabillity = PluginClass.Scp127Config.MaxAmmo;
                    else item.Durabillity = newdur;
                }
                yield return Timing.WaitForSeconds(PluginClass.Scp127Config.ReloadIntervall);
            }
        }
    }
}
