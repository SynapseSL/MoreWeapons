using Neuron.Core.Meta;
using Syml;

namespace MoreWeapons.Configs;

[Automatic]
[DocumentSection("XL Medkit")]
public class XlMedkitConfig : IDocumentSection
{
    public int AmountOfUsages { get; set; } = 3;

    public float HealAmount { get; set; } = 200f;
}