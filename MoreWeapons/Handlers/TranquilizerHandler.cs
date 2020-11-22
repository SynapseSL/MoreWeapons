using Synapse;
using MEC;
using Synapse.Api;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreWeapons.Handlers
{
    public class TranquilizerHandler
    {
        public TranquilizerHandler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                ID = (int)CustomItemType.Tranquilizer,
                Name = "Tranquilizer",
                BasedItemType = ItemType.GunUSP
            });

            Server.Get.Events.Player.LoadComponentsEvent += LoadComponents;
            Server.Get.Events.Player.PlayerPickUpItemEvent += Pickup;
            Server.Get.Events.Player.PlayerDropItemEvent += OnDrop;
            Server.Get.Events.Player.PlayerShootEvent += OnShoot;
            Server.Get.Events.Player.PlayerReloadEvent += OnReload;
            Server.Get.Events.Player.PlayerItemUseEvent += OnItem;
        }

        private void OnItem(Synapse.Api.Events.SynapseEventArguments.PlayerItemInteractEventArgs ev)
        {
            if (!ev.Player.GetComponent<TranquilizerPlayerScript>().Stuned) return;

            ev.Allow = false;
        }

        private void OnReload(Synapse.Api.Events.SynapseEventArguments.PlayerReloadEventArgs ev)
        {
            if (ev.Item.ID == (int)CustomItemType.Tranquilizer)
            {
                ev.Allow = false;

                if (ev.Item.Durabillity >= PluginClass.TzConfig.MagazineSize || PluginClass.TzConfig.Reloadable) return;

                var reloadAmount = PluginClass.TzConfig.MagazineSize - ev.Item.Durabillity;

                if (ev.Player.Ammo9 < reloadAmount * PluginClass.TzConfig.AmooNeededForOneShoot)
                    reloadAmount = ev.Player.Ammo9 / PluginClass.TzConfig.AmooNeededForOneShoot;

                ev.Item.Durabillity += reloadAmount;
                ev.Player.Ammo9 -= (uint)reloadAmount * (uint)PluginClass.TzConfig.AmooNeededForOneShoot;
            }
        }

        private void OnShoot(Synapse.Api.Events.SynapseEventArguments.PlayerShootEventArgs ev)
        {
            if(ev.Weapon.ID == (int)CustomItemType.Tranquilizer)
            {
                if (ev.Target != null)
                {
                    ev.Target.GetComponent<TranquilizerPlayerScript>().Stun();
                    ev.Weapon.Durabillity--;
                    ev.Allow = false;
                }
            }
        }

        private void OnDrop(Synapse.Api.Events.SynapseEventArguments.PlayerDropItemEventArgs ev)
        {
            if (!ev.Player.GetComponent<TranquilizerPlayerScript>().Stuned) return;

            ev.Allow = false;
        }

        private void Pickup(Synapse.Api.Events.SynapseEventArguments.PlayerPickUpItemEventArgs ev)
        {
            if (ev.Item.ID == (int)CustomItemType.Tranquilizer)
                ev.Player.GiveTextHint("You have picked up a Tranquilizer");
        }

        private void LoadComponents(Synapse.Api.Events.SynapseEventArguments.LoadComponentEventArgs ev)
        {
            if (ev.Player.GetComponent<TranquilizerPlayerScript>() == null)
                ev.Player.AddComponent<TranquilizerPlayerScript>();
        }
    }

    public class TranquilizerPlayerScript : MonoBehaviour
    {
        public TranquilizerPlayerScript() => Player = this.GetComponent<Player>();

        private readonly Player Player;

        public bool Stuned { get; private set; }

        public void Stun() => Timing.RunCoroutine(_stun());

        private IEnumerator<float> _stun()
        {
            if (PluginClass.TzConfig.BlockedIDs.Any(x => Player.RoleID == x)) yield break;

            Player.GiveTextHint("You are <color=blue>stuned</color> by a <color=blue>Tranquilizer</color>");

            Synapse.Api.Ragdoll rag = null;
            if (PluginClass.TzConfig.SpawnRagdoll)
                rag = Map.Get.CreateRagdoll(Player.RoleType, Player.Position, Quaternion.identity, Vector3.zero, new PlayerStats.HitInfo(), false, Player);

            if (PluginClass.TzConfig.DropInventory)
                Player.Inventory.DropAll();

            var pos = Player.Position;
            Player.GodMode = true;
            Player.Position = Vector3.up;

            Stuned = true;

            yield return Timing.WaitForSeconds(UnityEngine.Random.Range(PluginClass.TzConfig.MinStunTime, PluginClass.TzConfig.MaxStunTime));

            if (rag != null) rag.Destroy();
            Player.GodMode = false;
            Player.Position = pos;
            Stuned = false;

            if (Map.Get.Nuke.Detonated)
            {
                yield return Timing.WaitForSeconds(0.1f);
                Player.Hurt(99999);
            }
        }
    }
}
