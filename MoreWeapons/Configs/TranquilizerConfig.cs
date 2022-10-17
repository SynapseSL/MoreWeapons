using System;
using System.Collections.Generic;
using System.ComponentModel;
using Neuron.Core.Meta;
using Syml;

namespace MoreWeapons.Configs;

[Automatic]
[Serializable]
[DocumentSection("Tranquilizer")]
public class TranquilizerConfig : IDocumentSection
{
    [Description("The Amount of bullets that can be in the Tranquilizer")]
    public int MagazineSize { get; set; } = 1;

    [Description("The Ammount of Ammo9 needed to reload one tranquilizer bullet")]
    public int AmmoNeededForOneShoot { get; set; } = 10;

    [Description("The Maximal Amount of time a Player can be stuned")]
    public float MaxStunTime { get; set; } = 15f;

    [Description("The Minimal Amount of time a Player is stuned")]
    public float MinStunTime { get; set; } = 10f;

    [Description("If Enabled the Tranquilizer can be Reloaded with 18 Ammo9")]
    public bool Reloadable { get; set; } = true;

    [Description("All RoleIds in this List can't be stuned (also CustomRoles)")]
    public List<uint> BlockedIDs { get; set; } = new() { (uint)RoleType.Scp106, (uint)RoleType.Scp173 };
}
