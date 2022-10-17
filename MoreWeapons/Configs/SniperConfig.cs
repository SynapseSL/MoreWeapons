using System.ComponentModel;
using Syml;

namespace MoreWeapons.Configs;

//[Automatic]
[DocumentSection("Sniper")]
public class SniperConfig : IDocumentSection
{
    [Description("The max amount of bullets that can be in the sniper")]
    public int MagazineSize { get; set; } = 1;

    [Description("The Ammount of Ammo5 needed to reload one sniper bullet")]
    public int AmmoNeededForOneShoot { get; set; } = 10;

    [Description("The damage which the sniper does")]
    public float Damage { get; set; } = 150f;

    [Description("The Delay between the Shots the Player needs to wait")]
    public float DelayBetweenShots { get; set; } = 5f;
}
