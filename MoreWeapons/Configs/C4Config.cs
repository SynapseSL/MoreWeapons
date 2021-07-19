using System.ComponentModel;
using Synapse.Config;

namespace MoreWeapons.Configs
{
    public class C4Config : AbstractConfigSection
    {
        [Description("After how much seconds the C4 will explode by itself")]
        public float FuseTime { get; set; } = 200f;

        [Description("When Enabled the Grenade will follow the object it is sticked on")]
        public bool UpdatePosition { get; set; } = true;
    }
}
