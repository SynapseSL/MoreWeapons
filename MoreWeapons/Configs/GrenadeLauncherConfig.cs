using Synapse.Config;
using System.ComponentModel;

namespace MoreWeapons.Configs
{
    public class GrenadeLauncherConfig : AbstractConfigSection
    {
        [Description("If the grenade launcher can be reloaded with grenades")]
        public bool CanBeReloaded = true;

        [Description("How many grenades can be loaded into the grenade launcher")]
        public int MagazineSize = 10;

        [Description("With how much force the Grenades should fly")]
        public float ForceMultiplier = 1;

        [Description("After what amount of time the grenades Explodes")]
        public float GrenadeFuseTime = 0.75f;
    }
}
