using System.ComponentModel;
using Synapse.Config;
using System.Collections.Generic;

namespace MoreWeapons.Configs
{
    public class VaccinePistoleConfig : AbstractConfigSection
    {
        [Description("The Roles that SCP-049-2 can be replaced with when hit by the VaccinePistole")]
        public List<int> ReplaceRoles = new List<int>
        {
            (int)RoleType.ClassD,
            (int)RoleType.Scientist
        };

        [Description("The amount of Damage when the Target is not SCP-049-2")]
        public int Damage = 10;

        [Description("The Amount of Adrenaline that can be loaded into the VaccinePistole")]
        public int MagazineSize { get; set; }
    }
}
