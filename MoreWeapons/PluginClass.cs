using Synapse.Api.Plugin;

namespace MoreWeapons
{
    [PluginInformation(
        Name = "MoreWeapons",
        Author = "Dimenzio",
        Description = "A Plugin that adds new Weapons & Items to the Game",
        LoadPriority = 100,
        SynapseMajor = SynapseController.SynapseMajor,
        SynapseMinor = SynapseController.SynapseMinor,
        SynapsePatch = SynapseController.SynapsePatch
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

        public override void Load()
        {
            new Handlers.GrenadeLauncherHandler();
            new Handlers.ShotGunHandler();
            new Handlers.SniperHandler();
            new Handlers.Scp127Handler();
            new Handlers.XlMedkitHandler();
            new Handlers.TranquilizerHandler();
        }
    }
}
