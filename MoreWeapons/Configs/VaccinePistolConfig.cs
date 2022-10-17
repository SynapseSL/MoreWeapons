using System.Collections.Generic;
using System.ComponentModel;
using Neuron.Core.Meta;
using Syml;

namespace MoreWeapons.Configs;

//[Automatic]
[DocumentSection("VaccinePistol")]
public class VaccinePistolConfig : IDocumentSection
{
    [Description("The Roles that SCP-049-2 can be replaced with when hit by the VaccinePistole")]
    public List<RoleType> ReplaceRoles { get; set; } = new()
    {
        RoleType.ClassD,
        RoleType.Scientist,
        RoleType.FacilityGuard,
    };

    [Description("The amount of Heal when the Target is not SCP-049-2")]
    public int Heal { get; set; } = 150;

    [Description("The Amount of SCP-500 that can be loaded into the VaccinePistole")]
    public int MagazineSize { get; set; } = 3;

    public bool Reloadable { get; set; } = true;
    
    public ItemType UsedAmmo { get; set; } = ItemType.SCP500;
}
