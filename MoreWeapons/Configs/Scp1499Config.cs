using Synapse.Config;
using System.ComponentModel;

namespace MoreWeapons.Configs
{
    public class Scp1499Config : AbstractConfigSection
    {
        [Description("Where a player lands using Scp1499")]
        public SerializedMapPoint Scp1499Dimension = new Synapse.Config.SerializedMapPoint("HCZ_049", -13f, 266f, 0f);

        [Description("Spawns a Door at the default spawn so that the Player can't see in the void")]
        public bool SpawnDoor = true;

        [Description("The time after which the Player gets forced out of Scp1499")]
        public float Scp1499ResidenceTime = -1f;

        [Description("The Cooldown that Scp1499 gives")]
        public float Cooldown = 5f;
    }
}
