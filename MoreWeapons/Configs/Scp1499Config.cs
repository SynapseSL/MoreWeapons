using Synapse.Config;
using System.ComponentModel;

namespace MoreWeapons.Configs
{
    public class Scp1499Config : AbstractConfigSection
    {
        [Description("Where a player lands using Scp1499")]
        public SerializedMapPoint Scp1499Dimension = new Synapse.Config.SerializedMapPoint("HCZ_049", -13f, 266f, 0f);

        public float Scp1499ResidenceTime = -1f;
    }
}
