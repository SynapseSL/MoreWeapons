using Synapse;
using Synapse.Api;

namespace MoreWeapons.Handlers
{
    public class MedkitGunHandler
    {
        public MedkitGunHandler()
        {
            Server.Get.ItemManager.RegisterCustomItem(new Synapse.Api.Items.CustomItemInformation
            {
                BasedItemType = ItemType.GunMP7,
                ID = (int)CustomItemType.MedkitGun,
                Name = "MedkitGun"
            });
        }
    }
}
