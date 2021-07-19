using System.Collections.Generic;
using System.Linq;
using MEC;
using MoreWeapons.Scripts;
using Synapse;
using Synapse.Api;
using Synapse.Api.Events.SynapseEventArguments;
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
            Server.Get.Events.Player.PlayerDropAmmoEvent += OnDropAmmo;
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
        //C4
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
                    ev.Player.Inventory.AddItem(ItemType.Medkit, 0, 0, 0, 0);
                    break;

                case (int)CustomItemType.Scp1499 when ev.State == Synapse.Api.Events.SynapseEventArguments.ItemInteractState.Finalizing:
                    ev.Player.GetComponent<Scp1499PlayerScript>().Use1499();
                    ev.Allow = false;
                    break;

                case (int)CustomItemType.C4 when ev.State == Synapse.Api.Events.SynapseEventArguments.ItemInteractState.Finalizing:
                    ev.Allow = false;
                    ev.CurrentItem.Destroy();

                    var grenade = ev.Player.GrenadeManager.availableGrenades[0].grenadeInstance.GetComponent<Grenades.Grenade>();
                    var pos = ev.Player.CameraReference.TransformPoint(grenade.throwStartPositionOffset);
                    var vel = grenade.throwForce * (ev.Player.CameraReference.forward + grenade.throwLinearVelocityOffset).normalized;

                    var grenadeobj = Map.Get.SpawnGrenade(pos, vel, PluginClass.C4Config.FuseTime, Synapse.Api.Enum.GrenadeType.Grenade, ev.Player);
                    var component = grenadeobj.gameObject.AddComponent<StickyComponent>();
                    component.owner = ev.Player.gameObject;
                    component.grenade = grenadeobj;

                    ev.Player.GetComponent<C4PlayerComponent>().C4.Add(grenadeobj);
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
                        ev.Player.WeaponManager.RpcConfirmShot(true, ev.Player.WeaponManager.curWeapon);
                    }
                    else ev.Player.WeaponManager.RpcConfirmShot(false, ev.Player.WeaponManager.curWeapon);
                    break;

                case (int)CustomItemType.MedkitGun:
                    ev.Player.PlayerInteract.OnInteract();
                    ev.Allow = false;
                    ev.Weapon.Durabillity--;

                    if (ev.Target != null)
                    {
                        ev.Target.Heal(PluginClass.MedkitGunConfig.HealAmount);
                        ev.Player.WeaponManager.RpcConfirmShot(true, ev.Player.WeaponManager.curWeapon);
                    }
                    else ev.Player.WeaponManager.RpcConfirmShot(false, ev.Player.WeaponManager.curWeapon);
                    break;

                case (int)CustomItemType.GrenadeLauncher:
                    ev.Player.PlayerInteract.OnInteract();
                    ev.Allow = false;
                    ev.Weapon.Durabillity--;

                    var velocity = (ev.TargetPosition - ev.Player.Position) * PluginClass.GLConfig.ForceMultiplier;
                    var pos = ev.Player.CameraReference.TransformPoint(
                        ev.Player.GrenadeManager.availableGrenades[0].grenadeInstance.GetComponent<Grenades.Grenade>().throwStartPositionOffset);

                    var grenade = Map.Get.SpawnGrenade(pos, velocity, PluginClass.GLConfig.GrenadeFuseTime, Synapse.Api.Enum.GrenadeType.Grenade, ev.Player);

                    if (PluginClass.GLConfig.ExplodeOnCollison)
                    {
                        var script = grenade.gameObject.AddComponent<ExplodeScript>();
                        script.owner = ev.Player.gameObject;
                    }
                    break;

                case (int)CustomItemType.ShotGun:
                    ev.Player.PlayerInteract.OnInteract();
                    ev.Allow = false;

                    var bullets = PluginClass.SGConfig.BulletsPerShoot;
                    if (ev.Weapon.Durabillity <= bullets)
                        bullets = (int)ev.Weapon.Durabillity;
                    var rays = new Ray[bullets];
                    for (int i = 0; i < rays.Length; i++)
                        rays[i] = new Ray(ev.Player.CameraReference.position + ev.Player.CameraReference.forward, RandomAimcone() * ev.Player.CameraReference.forward);

                    var hits = new RaycastHit[bullets];
                    var didHit = new bool[hits.Length];
                    for (int i = 0; i < hits.Length; i++)
                        didHit[i] = Physics.Raycast(rays[i], out hits[i], 500f, 1208246273);

                    var component = ev.Player.GetComponent<WeaponManager>();
                    var confirm = false;
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (!didHit[i]) continue;

                        var hitbox = hits[i].collider.GetComponent<HitboxIdentity>();
                        if (hitbox != null)
                        {
                            var target = hits[i].collider.GetComponentInParent<Synapse.Api.Player>();

                            if (component.GetShootPermission(target.ClassManager))
                            {
                                int damage;
                                switch (hitbox.id)
                                {
                                    case HitBoxType.HEAD: damage = PluginClass.SGConfig.DamageHead; break;
                                    case HitBoxType.ARM: damage = PluginClass.SGConfig.DamageArm; break;
                                    case HitBoxType.LEG: damage = PluginClass.SGConfig.DamageLeg; break;
                                    default: damage = PluginClass.SGConfig.DamageBody; break;
                                }
                                if (target.RoleType == RoleType.Scp106)
                                    damage /= 10;

                                target.Hurt(damage, DamageTypes.Mp7, ev.Player);
                                component.RpcPlaceDecal(true, (sbyte)target.ClassManager.Classes.SafeGet(target.RoleType).bloodType, hits[i].point + hits[i].normal * 0.01f, Quaternion.FromToRotation(Vector3.up, hits[i].normal));
                                confirm = true;
                            }

                            continue;
                        }

                        var window = hits[i].collider.GetComponent<BreakableWindow>();
                        if (window != null)
                        {
                            window.ServerDamageWindow(PluginClass.SGConfig.DamageBody);
                            confirm = true;
                            continue;
                        }

                        component.RpcPlaceDecal(false, component.curWeapon, hits[i].point + hits[i].normal * 0.01f, Quaternion.FromToRotation(Vector3.up, hits[i].normal));
                    }

                    for (int i = 0; i < bullets; i++)
                        component.RpcConfirmShot(confirm, component.curWeapon);

                    ev.Weapon.Durabillity -= bullets;
                    break;

                case (int)CustomItemType.VaccinePistole:
                    ev.Player.PlayerInteract.OnInteract();
                    ev.Allow = false;
                    ev.Weapon.Durabillity--;

                    if (ev.Target != null)
                    {
                        ev.Player.WeaponManager.RpcConfirmShot(true, ev.Player.WeaponManager.curWeapon);
                        if (ev.Target.RoleID == (int)RoleType.Scp0492)
                        {
                            pos = ev.Target.Position;
                            ev.Target.RoleType = RoleType.ClassD;
                            ev.Target.Position = pos;
                        }
                        else ev.Target.Hurt(10);
                    }
                    else ev.Player.WeaponManager.RpcConfirmShot(false, ev.Player.WeaponManager.curWeapon);
                    break;
            }
        }

        //Used by:
        //Tranquilizer
        //MedkitGun
        //SCP-127
        //Sniper
        //GrenadeLauncher
        //ShotGun
        private void OnReload(PlayerReloadEventArgs ev)
        {
            switch (ev.Item?.ID)
            {
                case (int)CustomItemType.Tranquilizer:
                    ev.Allow = false;

                    if (ev.Item.Durabillity >= PluginClass.TzConfig.MagazineSize || !PluginClass.TzConfig.Reloadable) return;

                    var reloadAmount = PluginClass.TzConfig.MagazineSize - ev.Item.Durabillity;

                    if (ev.Player.Ammo9 < reloadAmount * PluginClass.TzConfig.AmooNeededForOneShoot)
                        reloadAmount = ev.Player.Ammo9 / PluginClass.TzConfig.AmooNeededForOneShoot;

                    ev.Item.Durabillity += reloadAmount;
                    ev.Player.Ammo9 -= (uint)reloadAmount * (uint)PluginClass.TzConfig.AmooNeededForOneShoot;
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

                    if (ev.Player.Ammo5 < reloadAmount * PluginClass.SnConfig.AmooNeededForOneShoot)
                        reloadAmount = ev.Player.Ammo5 / PluginClass.SnConfig.AmooNeededForOneShoot;

                    ev.Item.Durabillity += reloadAmount;
                    ev.Player.Ammo5 -= (uint)reloadAmount * (uint)PluginClass.SnConfig.AmooNeededForOneShoot;
                    break;

                case (int)CustomItemType.GrenadeLauncher:
                    ev.Allow = false;

                    if (!PluginClass.GLConfig.CanBeReloaded)
                        return;

                    foreach (var grenade in ev.Player.Inventory.Items.Where(x => x.ID == (int)ItemType.GrenadeFrag))
                    {
                        if (ev.Item.Durabillity >= PluginClass.GLConfig.MagazineSize)
                            return;

                        grenade.Destroy();
                        ev.Item.Durabillity++;
                    }
                    break;

                case (int)CustomItemType.ShotGun:
                    ev.Allow = false;

                    if (ev.Item.Durabillity >= PluginClass.SGConfig.MagazineSize) return;

                    reloadAmount = PluginClass.SGConfig.MagazineSize - ev.Item.Durabillity;

                    if (ev.Player.Ammo9 < reloadAmount)
                        reloadAmount = ev.Player.Ammo9;

                    ev.Item.Durabillity += reloadAmount;
                    ev.Player.Ammo9 -= (uint)reloadAmount;
                    break;
            }
        }

        //Used by:
        //Tranquilizer
        //SCP-1499
        //C4
        private void OnLoadComponents(LoadComponentEventArgs ev)
        {
            if (ev.Player.GetComponent<TranquilizerPlayerScript>() == null)
                ev.Player.AddComponent<TranquilizerPlayerScript>();

            if (ev.Player.GetComponent<Scp1499PlayerScript>() == null)
                ev.Player.AddComponent<Scp1499PlayerScript>();

            if (ev.Player.GetComponent<C4PlayerComponent>() == null)
                ev.Player.AddComponent<C4PlayerComponent>();
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
                case (int)CustomItemType.Sniper when ev.HitInfo.GetDamageType() == DamageTypes.E11StandardRifle:
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
                var door = Synapse.Api.Door.SpawnDoorVariant(doorSpawn.Parse().Position, Synapse.Api.Map.Get.GetRoom(RoomInformation.RoomType.HCZ_049).GameObject.transform.rotation);
                door.GameObject.GetComponent<Interactables.Interobjects.DoorUtils.DoorVariant>().ServerChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason.SpecialDoorFeature, true);
            }
        }

        //Used by:
        //SCP-1499
        private void OnSetClass(Synapse.Api.Events.SynapseEventArguments.PlayerSetClassEventArgs ev)
        {
            ev.Player.GetComponent<Scp1499PlayerScript>().IsInDimension = false;
        }

        //Used by:
        //C4
        private void OnDropAmmo(Synapse.Api.Events.SynapseEventArguments.PlayerDropAmmoEventArgs ev)
        {
            if (ev.Player.GetComponent<C4PlayerComponent>().ExplodeAll())
                ev.Allow = false;
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

        private Quaternion RandomAimcone()
        {
            return Quaternion.Euler(
                UnityEngine.Random.Range(-PluginClass.SGConfig.AimCone, PluginClass.SGConfig.AimCone),
                UnityEngine.Random.Range(-PluginClass.SGConfig.AimCone, PluginClass.SGConfig.AimCone),
                UnityEngine.Random.Range(-PluginClass.SGConfig.AimCone, PluginClass.SGConfig.AimCone)
                );
        }
    }
}
