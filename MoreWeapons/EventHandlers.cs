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
