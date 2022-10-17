using Neuron.Core.Events;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Map.Rooms;
using UnityEngine;

namespace MoreWeapons;

public class EventHandlers : Listener
{
    private readonly RoomService _roomService;
    private readonly MoreWeapons _moreWeapons;
    
    public SynapseCustomRoom Scp1499Dimension { get; private set; }

    public EventHandlers(PlayerEvents playerEvents, ItemEvents itemEvents, ScpEvents scpEvents,
        RoomService roomService, MoreWeapons moreWeapons)
    {
        _roomService = roomService;
        _moreWeapons = moreWeapons;
        
        playerEvents.Pickup.Subscribe(PlayerInteract);
        playerEvents.ChangeItem.Subscribe(PlayerInteract);
        playerEvents.DropAmmo.Subscribe(PlayerInteract);
        playerEvents.DropItem.Subscribe(PlayerInteract);
        playerEvents.FreePlayer.Subscribe(PlayerInteract);
        playerEvents.GeneratorInteract.Subscribe(PlayerInteract);
        playerEvents.LockerUse.Subscribe(PlayerInteract);
        playerEvents.StartWarhead.Subscribe(PlayerInteract);
        playerEvents.CallVanillaElevator.Subscribe(PlayerInteract);
        playerEvents.OpenWarheadButton.Subscribe(PlayerInteract);
        playerEvents.StartWorkStation.Subscribe(PlayerInteract);
        itemEvents.BasicInteract.Subscribe(PlayerInteract);
        scpEvents.Scp049Attack.Subscribe(ScpAttack);
        scpEvents.Scp0492Attack.Subscribe(ScpAttack);
        scpEvents.Scp096Attack.Subscribe(ScpAttack);
        scpEvents.Scp106Attack.Subscribe(ScpAttack);
        scpEvents.Scp173Attack.Subscribe(ScpAttack);
        scpEvents.Scp939Attack.Subscribe(ScpAttack);
    }

    private void ScpAttack(ScpAttackEvent ev)
    {
        if (ev.Scp?.GetComponent<MoreWeaponsScript>()?.CurrentlyStunned == true)
        {
            ev.Allow = false;
        }
    }

    private void PlayerInteract(PlayerInteractEvent ev)
    {
        if (ev.Player?.GetComponent<MoreWeaponsScript>()?.CurrentlyStunned == true)
        {
            ev.Allow = false;
        }
    }

    [EventHandler]
    public void SetClass(SimpleSetClassEvent ev) => ev.Player.GetComponent<MoreWeaponsScript>().In1499Dimension = false;

    [EventHandler]
    public void Waiting(RoundWaitingEvent ev)
    {
        Scp1499Dimension = _roomService.SpawnCustomRoom(1499, _moreWeapons.Scp1499Config.DimensionPosition);
    }

    [EventHandler]
    public void Revive(ReviveEvent ev)
    {
        if (ev.Scp049?.GetComponent<MoreWeaponsScript>()?.CurrentlyStunned == true)
        {
            ev.Allow = false;
        }
    }

    [EventHandler]
    public void DoorInteract(DoorInteractEvent ev)
    {
        if (ev.Player.GetComponent<MoreWeaponsScript>().CurrentlyStunned)
        {
            ev.Allow = false;
            ev.PlayDeniedSound = false;
        }
    }

    [EventHandler]
    public void LoadComponent(LoadComponentEvent ev) => ev.AddComponent<MoreWeaponsScript>();

    [EventHandler]
    public void Test(KeyPressEvent ev)
    {
        switch (ev.KeyCode)
        {
            case KeyCode.Alpha1:
                ev.Player.GetComponent<MoreWeaponsScript>().Stun();
                break;
        }
    }
}
    
    /*
    public class EventHandlers
    {

        //Used by:
        //Tranquilizer
        //GrenadLauncher
        //MedkitGun
        //VaccinePistole
        private void OnShoot(PlayerShootEventArgs ev)
        {
            switch (ev.Weapon?.ID)
            {
                case (int)CustomItemType.MedkitGun:
                    ev.Player.PlayerInteract.OnInteract();
                    ev.Allow = false;
                    ev.Weapon.Durabillity--;

                    if (ev.Target != null)
                    {
                        ev.Target.Heal(PluginClass.MedkitGunConfig.HealAmount);
                        Hitmarker.SendHitmarker(ev.Player.Connection, 1f);
                    }
                    break;

                case (int)CustomItemType.VaccinePistol:
                    ev.Player.PlayerInteract.OnInteract();
                    ev.Allow = false;
                    ev.Weapon.Durabillity--;

                    if (ev.Target != null)
                    {
                        Hitmarker.SendHitmarker(ev.Player.Connection, 1f);

                        if (ev.Target.RoleID == (int)RoleType.Scp0492)
                        {
                            var pos = ev.Target.Position;
                            ev.Target.ChangeRoleAtPosition(PluginClass.VPConfig.ReplaceRoles.ElementAt(Random.Range(0, PluginClass.VPConfig.ReplaceRoles.Count)));
                            ev.Target.Position = pos;
                        }
                        else
                        {
                            ev.Target.Hurt(PluginClass.VPConfig.Heal);
                            ev.Target.PlayerEffectsController.UseMedicalItem(ItemType.SCP500);
                        }
                    }
                    break;
            }
        }

        //Used by:
        //Tranquilizer
        //MedkitGun
        //SCP-127
        //Sniper
        //GrenadeLauncher
        //VaccinePistole
        private void OnReload(PlayerReloadEventArgs ev)
        {
            switch (ev.Item?.ID)
            {

                case (int)CustomItemType.MedkitGun:
                    ev.Allow = false;

                    if (!PluginClass.MedkitGunConfig.CanBeReloaded)
                        return;

                    foreach (var medkit in ev.Player.Inventory.Items.Where(x => x.ID == (int)ItemType.Medkit))
                    {
                        if (ev.Item.Durabillity >= PluginClass.MedkitGunConfig.MagazineSize)
                            return;

                        medkit.Destroy();
                        ev.Item.Durabillity++;
                    }
                    break;

                case (int)CustomItemType.VaccinePistol:
                    ev.Allow = false;

                    foreach (var scp500 in ev.Player.Inventory.Items.Where(x => x.ID == (int)ItemType.SCP500))
                    {
                        if (ev.Item.Durabillity >= PluginClass.VPConfig.MagazineSize)
                            return;

                        scp500.Destroy();
                        ev.Item.Durabillity++;
                    }
                    break;
            }
        }
    }
    */
