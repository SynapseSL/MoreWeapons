using Synapse.Api.Plugin;
using Synapse;

namespace MoreWeapons
{
    [PluginInformation(
        Name = "MoreWeapons",
        Author = "Dimenzio",
        Description = "A Plugin that adds new Weapons & Items to the Game",
        LoadPriority = 100,
        SynapseMajor = 2,
        SynapseMinor = 4,
        SynapsePatch = 2,
        Version = "1.1.3"
        )]
    public class PluginClass : AbstractPlugin
    {
        [Config(section = "GrenadeLauncher")]
        public static Configs.GrenadeLauncherConfig GLConfig;

        [Config(section = "ShotGun")]
        public static Configs.ShotGunConfig SGConfig;

        [Config(section = "Scp-127")]
        public static Configs.Scp127Config Scp127Config;

        [Config(section = "Sniper")]
        public static Configs.SniperConfig SnConfig;

        [Config(section = "Tranquilizer")]
        public static Configs.TranquilizerConfig TzConfig;

        [Config(section = "Scp1499")]
        public static Configs.Scp1499Config Scp1499Config;

        [Config(section = "C4")]
        public static Configs.C4Config C4Config;

        public override void Load()
        {
            new EventHandlers();
            new Handlers.GrenadeLauncherHandler();
            new Handlers.ShotGunHandler();
            new Handlers.SniperHandler();
            new Handlers.Scp127Handler();
            new Handlers.XlMedkitHandler();
            new Handlers.TranquilizerHandler();
            new Handlers.Scp1499Handler();
            new Handlers.MedkitGunHandler();
            new Handlers.C4Handler();
        }
    }
}
