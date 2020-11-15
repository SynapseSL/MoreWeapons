using Synapse.Config;

namespace MoreWeapons.Configs
{
    public class ShotGunConfig : AbstractConfigSection
    {
        public int MagazineSize = 8;

        public int BulletsPerShoot = 8;

        public float AimCone = 5;

        public int DamageHead = 15;

        public int DamageArm = 5;

        public int DamageLeg = 5;

        public int DamageBody = 8;
    }
}
