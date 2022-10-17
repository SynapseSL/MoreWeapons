using System.ComponentModel;
using Neuron.Core.Meta;
using Syml;

namespace MoreWeapons.Configs;

[Automatic]
[DocumentSection("Grenade Launcher")]
public class GrenadeLauncherConfig : IDocumentSection
{
    [Description("If the grenade launcher can be reloaded with grenades")]
    public bool CanBeReloaded { get; set; } = true;

    [Description("How many grenades can be loaded into the grenade launcher")]
    public int MagazineSize { get; set; } = 3;

    [Description("With how much force the Grenades should fly")]
    public float ForceMultiplier { get; set; } = 1;

    [Description("After what amount of time the grenades Explodes")]
    public float GrenadeFuseTime { get; set; } = 2f;

    [Description("If Enabled the Grenade will Explode as soon as it hits any object")]
    public bool ExplodeOnCollision { get; set; } = true;
}
