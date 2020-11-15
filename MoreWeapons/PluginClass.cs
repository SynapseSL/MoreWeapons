using Synapse.Api.Plugin;

namespace MoreWeapons
{
    [PluginInformation(
        Name = "MoreWeapons",
        Author = "Dimenzio",
        Description = "A Plugin that adds new Weapons to the Game",
        LoadPriority = 100,
        SynapseMajor = SynapseController.SynapseMajor,
        SynapseMinor = SynapseController.SynapseMinor,
        SynapsePatch = 3
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

        public override void Load()
        {
            new Handlers.GrenadeLauncherHandler();
            new Handlers.ShotGunHandler();
            new Handlers.SniperHandler();
            new Handlers.Scp127Handler();
        }
    }
}
