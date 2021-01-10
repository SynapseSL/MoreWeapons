using Synapse;
using Synapse.Api;
using System.Linq;
using System.Collections.Generic;
using MEC;

namespace MoreWeapons.Handlers
{
    public class Scp127Handler
    {
        public Scp127Handler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                ID = (int)CustomItemType.Scp127,
                BasedItemType = ItemType.GunProject90,
                Name = "Scp-127"
            });

            Server.Get.Events.Player.PlayerReloadEvent += OnReload;
            Server.Get.Events.Round.RoundStartEvent += OnStart;
            Server.Get.Events.Round.RoundRestartEvent += OnRestart;
            Server.Get.Events.Player.PlayerPickUpItemEvent += OnPickup;
        }

        private void OnPickup(Synapse.Api.Events.SynapseEventArguments.PlayerPickUpItemEventArgs ev)
        {
            if (ev.Item?.ID == (int)CustomItemType.Scp127)
                ev.Player.GiveTextHint("You have picked up Scp-127");
        }

        private void OnStart() => coroutine = MEC.Timing.RunCoroutine(Refill());

        private MEC.CoroutineHandle coroutine;

        private IEnumerator<float> Refill()
        {
            for(; ; )
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

        private void OnRestart() => MEC.Timing.KillCoroutines(coroutine);

        private void OnReload(Synapse.Api.Events.SynapseEventArguments.PlayerReloadEventArgs ev)
        {
            if(ev.Item?.ID == (int)CustomItemType.Scp127)
                ev.Allow = false;
        }
    }
}
