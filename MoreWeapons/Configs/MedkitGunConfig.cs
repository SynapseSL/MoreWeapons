using Synapse.Config;
using System.ComponentModel;

namespace MoreWeapons.Configs
{
    public class MedkitGunConfig : AbstractConfigSection
    {
        [Description("The amount that the MedkitGun heals")]
        public float HealAmount = 150f;

        [Description("The amount of medkits that can be in one Medkit")]
        public int MagazineSize = 3;

        [Description("If the Medkitgun can be reloaded")]
        public bool CanBeReloaded = true;
    }
}
