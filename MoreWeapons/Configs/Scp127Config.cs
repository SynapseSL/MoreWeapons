using Synapse.Config;
using System.ComponentModel;

namespace MoreWeapons.Configs
{
    public class Scp127Config : AbstractConfigSection
    {
        [Description("The intervall in which Scp127 reloads ammo")]
        public float ReloadIntervall { get; set; } = 30f;

        [Description("The Amount of ammo that reloads each intervall")]
        public float ReloadAmount { get; set; } = 5;

        [Description("The max Amount of ammo it can reload")]
        public float MaxAmmo { get; set; } = 75;
    }
}
