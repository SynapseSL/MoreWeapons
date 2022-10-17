using System.ComponentModel;
using Neuron.Core.Meta;
using Syml;

namespace MoreWeapons.Configs;

[Automatic]
[DocumentSection("SCP 127")]
public class Scp127Config : IDocumentSection
{
    [Description("The intervall in which Scp127 reloads ammo")]
    public float ReloadInterval { get; set; } = 30f;

    [Description("The Amount of ammo that reloads each intervall")]
    public int ReloadAmount { get; set; } = 5;

    [Description("The max Amount of ammo it can reload")]
    public int MaxAmmo { get; set; } = 75;
}
