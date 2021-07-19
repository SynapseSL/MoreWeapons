using Synapse.Config;
using System.ComponentModel;

namespace MoreWeapons.Configs
{
    public class ShotGunConfig : AbstractConfigSection
    {
        [Description("The max amount of bullets that can be in the shotgun")]
        public int MagazineSize { get; set; } = 8;

        [Description("How many bullets get shoot every time")]
        public int BulletsPerShoot { get; set; } = 8;

        [Description("How big the spread should be")]
        public float AimCone { get; set; } = 5;

        [Description("The damage amount if you hit the head")]
        public int DamageHead { get; set; } = 15;

        [Description("The damage amount if you hit the arm")]
        public int DamageArm { get; set; } = 5;

        [Description("The damage amount if you hit the leg")]
        public int DamageLeg { get; set; } = 5;

        [Description("The damage amount if you hit the body")]
        public int DamageBody { get; set; } = 8;
    }
}
