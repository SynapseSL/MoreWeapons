using Synapse;
using Synapse.Api;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MoreWeapons.Handlers
{
    public class C4Handler
    {
        public C4Handler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                BasedItemType = ItemType.GrenadeFrag,
                ID = (int)CustomItemType.C4,
                Name = "C4"
            });

            Server.Get.Events.Player.LoadComponentsEvent += Components;
            Server.Get.Events.Player.PlayerItemUseEvent += UseItem;
            Server.Get.Events.Player.PlayerDropAmmoEvent += DropAmmo;
        }

        private void Components(Synapse.Api.Events.SynapseEventArguments.LoadComponentEventArgs ev)
        {
            if (ev.Player.GetComponent<C4PlayerComponent>() == null)
                ev.Player.AddComponent<C4PlayerComponent>();
        }

        private void DropAmmo(Synapse.Api.Events.SynapseEventArguments.PlayerDropAmmoEventArgs ev)
        {
            if (ev.Player.GetComponent<C4PlayerComponent>().ExplodeAll())
                ev.Allow = false;
        }

        private void UseItem(Synapse.Api.Events.SynapseEventArguments.PlayerItemInteractEventArgs ev)
        {
            if(ev.CurrentItem.ID == (int)CustomItemType.C4 && ev.State == Synapse.Api.Events.SynapseEventArguments.ItemInteractState.Finalizing)
            {
                ev.Allow = false;
                ev.CurrentItem.Destroy();

                var grenade = ev.Player.GrenadeManager.availableGrenades[0].grenadeInstance.GetComponent<Grenades.Grenade>();
                var pos = ev.Player.CameraReference.TransformPoint(grenade.throwStartPositionOffset);
                var vel = grenade.throwForce * (ev.Player.CameraReference.forward + grenade.throwLinearVelocityOffset).normalized;

                var grenadeobj = Map.Get.SpawnGrenade(pos, vel, PluginClass.C4Config.FuseTime, Synapse.Api.Enum.GrenadeType.Grenade, ev.Player);
                var component = grenadeobj.gameObject.AddComponent<StickyComponent>();
                component.owner = ev.Player.gameObject;
                component.grenade = grenadeobj;

                ev.Player.GetComponent<C4PlayerComponent>().c4.Add(grenadeobj);
            }
        }

        private class StickyComponent : MonoBehaviour
        {
            public GameObject owner;
            public Grenades.Grenade grenade;

            public Vector3 relativePosition;
            public GameObject collider = null;

            public void OnCollisionEnter(Collision col)
            {
                if (collider != null || col.gameObject == owner) return;

                var rig = gameObject.GetComponent<Rigidbody>();
                rig.useGravity = false;
                rig.isKinematic = true;
                grenade.NetworkserverVelocities = new Grenades.RigidbodyVelocityPair
                {
                    angular = Vector3.zero,
                    linear = Vector3.zero
                };

                if (!PluginClass.C4Config.UpdatePosition) return;

                relativePosition = col.gameObject.transform.InverseTransformPoint(gameObject.transform.position);
                collider = col.gameObject;
            }

            public void Update()
            {
                if (collider != null)
                    gameObject.transform.position = collider.transform.TransformPoint(relativePosition);
            }
        }

        private class C4PlayerComponent : MonoBehaviour
        {
            public List<Grenades.Grenade> c4 = new List<Grenades.Grenade>();

            public bool ExplodeAll()
            {
                var flag = false;
                foreach(var grenade in c4.ToList())
                {
                    c4.Remove(grenade);
                    if (grenade == null) continue;
                    grenade.NetworkfuseTime = 0.10000000149011612;
                    flag = true;
                }
                return flag;
            }
        }
    }
}
