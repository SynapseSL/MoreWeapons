using Neuron.Core.Meta;
using Synapse3.SynapseModule.Map.Rooms;

namespace MoreWeapons;

[Automatic]
[CustomRoom(
    Name = "Scp 1499 Dimension",
    Id = 1499,
    SchematicId = 1499
    )]
public class Scp1499Dimension : SynapseCustomRoom
{
    public override uint Zone => 1499;
}