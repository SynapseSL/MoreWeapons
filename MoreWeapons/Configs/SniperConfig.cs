using Synapse.Config;
using System.ComponentModel;

namespace MoreWeapons.Configs
{
    public class SniperConfig : AbstractConfigSection
    {
        [Description("The max amount of bullets that can be in the sniper")]
        public int MagazineSize { get; set; } = 1;

        [Description("The Ammount of Ammo5 needed to reload one sniper bullet")]
        public int AmooNeededForOneShoot { get; set; } = 10;

        [Description("The damage which the sniper does")]
        public float Damage { get; set; } = 150f;
    }
}
