using Synapse.Config;

namespace MoreWeapons.Configs
{
    public class SniperConfig : AbstractConfigSection
    {
        public int MagazineSize = 1;

        public int AmooNeededForOneShoot = 10;

        public float Damage = 150f;
    }
}
