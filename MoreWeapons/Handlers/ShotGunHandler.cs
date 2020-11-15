using Synapse;
using UnityEngine;
using Mirror;

namespace MoreWeapons.Handlers
{
    public class ShotGunHandler
    {
        public ShotGunHandler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                ID = (int)CustomItemType.ShotGun,
                Name = "ShotGun",
                BasedItemType = ItemType.GunUSP,
            });

            Server.Get.Events.Player.PlayerShootEvent += OnShoot;
            Server.Get.Events.Player.PlayerPickUpItemEvent += OnPickup;
            Server.Get.Events.Player.PlayerReloadEvent += OnReload;
        }

        private void OnReload(Synapse.Api.Events.SynapseEventArguments.PlayerReloadEventArgs ev)
        {
            if(ev.Item.ID == (int)CustomItemType.ShotGun)
            {
                ev.Allow = false;

                if (ev.Item.Durabillity >= PluginClass.SGConfig.MagazineSize) return;

                var reloadAmount = PluginClass.SGConfig.MagazineSize - ev.Item.Durabillity;

                if (ev.Player.Ammo9 < reloadAmount)
                    reloadAmount = ev.Player.Ammo9;

                ev.Item.Durabillity += reloadAmount;
                ev.Player.Ammo9 -= (uint)reloadAmount;
            }
        }

        private void OnPickup(Synapse.Api.Events.SynapseEventArguments.PlayerPickUpItemEventArgs ev)
        {
            if(ev.Item.ID == (int)CustomItemType.ShotGun)
                ev.Player.GiveTextHint("You have picked up a ShotGun");
        }

        private void OnShoot(Synapse.Api.Events.SynapseEventArguments.PlayerShootEventArgs ev)
        {
            if(ev.Weapon.ID == (int)CustomItemType.ShotGun)
            {
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
                for(int i =0; i < hits.Length; i++)
                {
                    if (!didHit[i]) continue;

                    var hitbox = hits[i].collider.GetComponent<HitboxIdentity>();
                    if(hitbox != null)
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

                            target.Hurt(damage, DamageTypes.Mp7, ev.Player);
                            component.RpcPlaceDecal(true, (sbyte)target.ClassManager.Classes.SafeGet(target.RoleType).bloodType, hits[i].point + hits[i].normal * 0.01f, Quaternion.FromToRotation(Vector3.up, hits[i].normal));
                            confirm = true;
                        }

                        continue;
                    }

                    var window = hits[i].collider.GetComponent<BreakableWindow>();
                    if(window != null)
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
