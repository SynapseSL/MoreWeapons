using System.Collections.Generic;
using Synapse;
using MEC;
using UnityEngine;

namespace MoreWeapons.Handlers
{
    public class Scp1499Handler
    {
        public Scp1499Handler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                BasedItemType = ItemType.SCP268,
                ID = (int)CustomItemType.Scp1499,
                Name = "Scp1499"
            });

            Server.Get.Events.Player.PlayerPickUpItemEvent += OnPickup;
            Server.Get.Events.Player.PlayerItemUseEvent += OnUse;
            Server.Get.Events.Player.LoadComponentsEvent += OnLoad;
            Server.Get.Events.Player.PlayerSetClassEvent += OnSetClass;
        }

        private void OnSetClass(Synapse.Api.Events.SynapseEventArguments.PlayerSetClassEventArgs ev) 
            => ev.Player.GetComponent<Scp1499PlayerScript>().IsInDimension = false;

        private void OnLoad(Synapse.Api.Events.SynapseEventArguments.LoadComponentEventArgs ev)
        {
            if (ev.Player.GetComponent<Scp1499PlayerScript>() == null)
                ev.Player.AddComponent<Scp1499PlayerScript>();
        }

        private void OnUse(Synapse.Api.Events.SynapseEventArguments.PlayerItemInteractEventArgs ev)
        {
            if(ev.CurrentItem != null && ev.CurrentItem.ID == (int)CustomItemType.Scp1499 && ev.State == Synapse.Api.Events.SynapseEventArguments.ItemInteractState.Finalizing)
            {
                ev.Player.GetComponent<Scp1499PlayerScript>().Use1499();
                ev.Allow = false;
            }
        }

        private void OnPickup(Synapse.Api.Events.SynapseEventArguments.PlayerPickUpItemEventArgs ev)
        {
            if (ev.Item.ID == (int)CustomItemType.Scp1499)
                ev.Player.GiveTextHint("You have picked up Scp1499");
        }
    }

    public class Scp1499PlayerScript : MonoBehaviour
    {
        private Synapse.Api.Player player;

        public Scp1499PlayerScript() => player = GetComponent<Synapse.Api.Player>();


        public Vector3 OldPosition { get; set; } = Vector3.up;

        public bool IsInDimension { get; set; } = false;

        public void Use1499()
        {
            if (IsInDimension)
            {
                player.Position = OldPosition;
                if (SynapseController.Server.Map.Decontamination.IsDecontaminationInProgress && player.Room.Zone == Synapse.Api.Enum.ZoneType.LCZ)
                    player.GiveEffect(Synapse.Api.Enum.Effect.Decontaminating);
                IsInDimension = false;

                Timing.KillCoroutines(kickcoroutine.ToArray());
            }
            else
            {
                player.GiveTextHint("You have entered the dimension of Scp1499");
                OldPosition = player.Position;
                player.Position = PluginClass.Scp1499Config.Scp1499Dimension.Parse().Position;

                KickOut(PluginClass.Scp1499Config.Scp1499ResidenceTime);
                IsInDimension = true;
            }
        }

        public void KickOut(float delay)
        {
            if (delay < 0f) return;
            kickcoroutine.Add(Timing.RunCoroutine(KickOutOfScp1499(delay)));
        }

        private List<CoroutineHandle> kickcoroutine = new List<CoroutineHandle>();

        private IEnumerator<float> KickOutOfScp1499(float delay)
        {
            yield return Timing.WaitForSeconds(delay);

            if (IsInDimension) Use1499();
        }
    }
}
