using System.Collections.Generic;
using System.ComponentModel;
using Synapse.Config;

namespace MoreWeapons.Configs
{
    public class TranquilizerConfig : AbstractConfigSection
    {
        [Description("The Amount of bullets that can be in the Tranquilizer")]
        public int MagazineSize { get; set; } = 1;

        [Description("The Ammount of Ammo9 needed to reload one tranquilizer bullet")]
        public int AmooNeededForOneShoot { get; set; } = 10;

        [Description("The Maximal Amount of time a Player can be stuned")]
        public float MaxStunTime { get; set; } = 15f;

        [Description("The Minimal Amount of time a Player is stuned")]
        public float MinStunTime { get; set; } = 10f;

        [Description("If Enabled a Player who got shoot at with a tranquilizer drops all his Items")]
        public bool DropInventory { get; set; } = false;

        [Description("If Enabled a Ragdoll will Spawn at the Position where the Player is tranquilized")]
        public bool SpawnRagdoll { get; set; } = true;

        [Description("If Enabled the Tranquilizer can be Reloaded with 18 Ammo9")]
        public bool Reloadable { get; set; } = true;

        [Description("All RoleIds in this List cant be stuned (also CustomRoles)")]
        public List<int> BlockedIDs { get; set; } = new List<int>() { (int)RoleType.Scp173, (int)RoleType.Scp106 };
    }
}
