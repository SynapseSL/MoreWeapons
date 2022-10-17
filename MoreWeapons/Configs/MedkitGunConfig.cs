using System.ComponentModel;
using Neuron.Core.Meta;
using Syml;

namespace MoreWeapons.Configs;

[Automatic]
[DocumentSection("MedkitGun")]
public class MedkitGunConfig : IDocumentSection
{
    [Description("The amount that the MedkitGun heals")]
    public float HealAmount { get; set; } = 150f;

    [Description("The amount of medkits that can be in one Medkit")]
    public int MagazineSize { get; set; } = 3;

    [Description("If the Medkitgun can be reloaded")]
    public bool CanBeReloaded { get; set; } = true;
}
