using Synapse;
using Synapse.Api.Items;
using Synapse.Api.Plugin;
using Synapse.Translation;

namespace MoreWeapons
{
    [PluginInformation(
        Name = "MoreWeapons",
        Author = "Dimenzio",
        Description = "A Plugin that adds new Weapons & Items to the Game",
        LoadPriority = 100,
        SynapseMajor = 2,
        SynapseMinor = 8,
        SynapsePatch = 3,
        Version = "1.3.2"
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

        [SynapseTranslation]
        public new static SynapseTranslation<PluginTranslation> Translation { get; set; }

        public override void Load()
        {
            RegisterItems();
            RegisterTranslation();

            new EventHandlers();
        }

        private void RegisterTranslation()
        {
            Translation.AddTranslation(new PluginTranslation());
            Translation.AddTranslation(new PluginTranslation
            {
                Drop1499 = "Du kannst SCP-1499 im moment nicht wegwerfen",
                Equipped = "Du hast ein(e) %item% ausgerüstet",
                PickedUp = "Du hast ein(e) %item% aufgehoben",
                Pocket1499 = "In dieser Dimension kannst du SCP-1499 nicht benutzen",
                Entered1499 = "Du hast die Dimension von SCP-1499 betreten",
                Tranquilized = "Du wurdest von der Tranquilizer betäubt und kannst dich nicht bewegen"
            }, "GERMAN");
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
