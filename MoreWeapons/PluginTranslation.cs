using Synapse.Translation;

namespace MoreWeapons
{
    public class PluginTranslation : IPluginTranslation
    {
        public string Equipped { get; set; } = "You have equipped a %item%";

        public string PickedUp { get; set; } = "You have picked up a %item%";

        public string Tranquilized { get; set; } = "You have been hit by the Tranquilizer and can't move";

        public string Entered1499 { get; set; } = "You have entered the dimension of SCP-1499";

        public string Drop1499 { get; set; } = "You can't currently drop SCP-1499";

        public string Pocket1499 { get; set; } = "You can't use SCP-1499 in this dimension";
    }
}
