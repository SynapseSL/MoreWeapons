using Synapse.Config;
using System.ComponentModel;

namespace MoreWeapons.Configs
{
    public class ShotGunConfig : AbstractConfigSection
    {
        [Description("The max amount of bullets that can be in the shotgun")]
        public int MagazineSize = 8;

        [Description("How many bullets get shoot every time")]
        public int BulletsPerShoot = 8;

        [Description("How big the spread should be")]
        public float AimCone = 5;

        [Description("The damage amount if you hit the head")]
        public int DamageHead = 15;

        [Description("The damage amount if you hit the arm")]
        public int DamageArm = 5;

        [Description("The damage amount if you hit the leg")]
        public int DamageLeg = 5;

        [Description("The damage amount if you hit the body")]
        public int DamageBody = 8;
    }
}
