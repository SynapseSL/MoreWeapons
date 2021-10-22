using System.Collections.Generic;
using InventorySystem.Items.Usables;
using MEC;
using Synapse.Api;
using Synapse.Api.Enum;
using UnityEngine;

namespace MoreWeapons.Scripts
{
    public class Scp1499PlayerScript : MonoBehaviour
    {
        private readonly Player player;

        public Scp1499PlayerScript() => player = GetComponent<Player>();

        public Vector3 OldPosition { get; set; } = Vector3.up;

        public bool IsInDimension { get; set; } = false;

        public void Use1499()
        {
            if (player.Zone == ZoneType.Pocket)
            {
                player.GiveTextHint(PluginClass.Translation.ActiveTranslation.Pocket1499);
                return;
            }

            if (IsInDimension)
            {
                player.Position = OldPosition;

                if (SynapseController.Server.Map.Decontamination.IsDecontaminationInProgress && player.Zone == ZoneType.LCZ)
                    player.GiveEffect(Effect.Decontaminating);

                IsInDimension = false;

                UsableItemsController.GetHandler(player.Hub).PersonalCooldowns[ItemType.SCP268] = PluginClass.Scp1499Config.Cooldown;

                Timing.KillCoroutines(kickcoroutine.ToArray());
            }
            else
            {
                player.GiveTextHint(PluginClass.Translation.ActiveTranslation.Entered1499);
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

        private readonly List<CoroutineHandle> kickcoroutine = new List<CoroutineHandle>();

        private IEnumerator<float> KickOutOfScp1499(float delay)
        {
            yield return Timing.WaitForSeconds(delay);

            if (IsInDimension)
                Use1499();
        }
    }
}
