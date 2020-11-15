using Synapse.Config;

namespace MoreWeapons.Configs
{
    public class GrenadeLauncherConfig : AbstractConfigSection
    {
        public bool CanBeReloaded = true;

        public int MagazineSize = 10;

        public float ForceMultiplier = 1;

        public float GrenadeFuseTime = 0.75f;
    }
}
