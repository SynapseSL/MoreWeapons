using Synapse;
using Synapse.Api.Items;
using Synapse.Api.Plugin;

namespace MoreWeapons
{
    [PluginInformation(
        Name = "MoreWeapons",
        Author = "Dimenzio",
        Description = "A Plugin that adds new Weapons & Items to the Game",
        LoadPriority = 100,
        SynapseMajor = 2,
        SynapseMinor = 6,
        SynapsePatch = 1,
        Version = "1.3.0"
        )]
    public class PluginClass : AbstractPlugin
    {
        [Config(section = "GrenadeLauncher")]
        public static Configs.GrenadeLauncherConfig GLConfig { get; set; }

        [Config(section = "Scp-127")]
        public static Configs.Scp127Config Scp127Config { get; set; }

        [Config(section = "Sniper")]
        public static Configs.SniperConfig SnConfig { get; set; }

        [Config(section = "Tranquilizer")]
        public static Configs.TranquilizerConfig TzConfig { get; set; }

        [Config(section = "Scp1499")]
        public static Configs.Scp1499Config Scp1499Config { get; set; }

        [Config(section = "MedkitGun")]
        public static Configs.MedkitGunConfig MedkitGunConfig { get; set; }

        [Config(section = "VaccinePistole")]
        public static Configs.VaccinePistoleConfig VPConfig { get; set; }

        public override void Load()
        {
            RegisterItems();

            new EventHandlers();
        }

        private void RegisterItems()
        {
            Server.Get.ItemManager.RegisterCustomItem(new CustomItemInformation
            {
                ID = (int)CustomItemType.Tranquilizer,
                Name = "Tranquilizer",
                BasedItemType = ItemType.GunCOM18
            });

            Server.Get.ItemManager.RegisterCustomItem(new CustomItemInformation
            {
                BasedItemType = ItemType.GunLogicer,
                ID = (int)CustomItemType.GrenadeLauncher,
                Name = "GrenadeLauncher"
            });

            Server.Get.ItemManager.RegisterCustomItem(new CustomItemInformation
            {
                ID = (int)CustomItemType.Sniper,
                BasedItemType = ItemType.GunE11SR,
                Name = "Sniper"
            });

            Server.Get.ItemManager.RegisterCustomItem(new CustomItemInformation
            {
                ID = (int)CustomItemType.Scp127,
                BasedItemType = ItemType.GunFSP9,
                Name = "Scp-127"
            });

            Server.Get.ItemManager.RegisterCustomItem(new CustomItemInformation
            {
                ID = (int)CustomItemType.XlMedkit,
                Name = "XlMedkit",
                BasedItemType = ItemType.Medkit
            });

            Server.Get.ItemManager.RegisterCustomItem(new CustomItemInformation
            {
                BasedItemType = ItemType.SCP268,
                ID = (int)CustomItemType.Scp1499,
                Name = "Scp1499"
            });

            Server.Get.ItemManager.RegisterCustomItem(new CustomItemInformation
            {
                BasedItemType = ItemType.GunFSP9,
                ID = (int)CustomItemType.MedkitGun,
                Name = "MedkitGun"
            });

            Server.Get.ItemManager.RegisterCustomItem(new CustomItemInformation
            {
                BasedItemType = ItemType.GunCOM15,
                ID = (int)CustomItemType.VaccinePistole,
                Name = "VaccinePistole"
            });
        }
    }
}
