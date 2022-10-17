using System.ComponentModel;
using Neuron.Core.Meta;
using Syml;
using Synapse3.SynapseModule.Config;

namespace MoreWeapons.Configs;

[Automatic]
[DocumentSection("SCP 1499")]
public class Scp1499Config : IDocumentSection
{
    [Description("Where The Schematic 1499 should spawn")]
    public SerializedVector3 DimensionPosition { get; set; } = new (0f, 900f, 0f);

    [Description("The time after which the Player gets forced out of Scp1499")]
    public float Scp1499ResidenceTime { get; set; } = 20f;

    [Description("The Cooldown that Scp1499 gives")]
    public float Cooldown { get; set; } = 5f;
}
