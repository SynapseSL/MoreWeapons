using System;
using Neuron.Core.Meta;
using Neuron.Modules.Configs.Localization;

namespace MoreWeapons;

[Automatic]
[Serializable]
public class MoreWeaponsTranslation : Translations<MoreWeaponsTranslation>
{
    public string Equipped { get; set; } = "You have equipped a %item%";

    public string PickedUp { get; set; } = "You have picked up a %item%";

    public string Tranquilized { get; set; } = "You have been hit by the Tranquilizer and can't move";

    public string SniperDelay { get; set; } = "You still have to wait %time% seconds";

    public string XLMedkitUSages { get; set; } = "The medkit can be used %amount% more times";

    public string Entered1499 { get; set; } = "You have entered the dimension of SCP-1499";

    public string Pocket1499 { get; set; } = "You can't use SCP-1499 in this dimension";
}
