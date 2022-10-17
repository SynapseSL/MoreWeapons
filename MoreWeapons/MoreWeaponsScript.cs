using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items.Usables;
using MEC;
using PlayerStatsSystem;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Item;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Map.Objects;
using Synapse3.SynapseModule.Map.Rooms;
using Synapse3.SynapseModule.Player;
using UnityEngine;

namespace MoreWeapons;

public class MoreWeaponsScript : MonoBehaviour
{
    public DecontaminationService DecontaminationService { get; private set; }
    
    public NukeService NukeService { get; private set; }

    public SynapsePlayer Player { get; private set; }
    
    public MoreWeapons MoreWeapons { get; private set; }

    public void Awake()
    {
        Player = GetComponent<SynapsePlayer>();
        MoreWeapons = Synapse.Get<MoreWeapons>();
        DecontaminationService = Synapse.Get<DecontaminationService>();
        NukeService = Synapse.Get<NukeService>();
    }

    public bool CurrentlyStunned { get; private set; }
    public void Stun() => Timing.RunCoroutine(_stun());
    private IEnumerator<float> _stun()
    {
        if (MoreWeapons.TzConfig.BlockedIDs.Contains(Player.RoleID)) yield break;
        if (CurrentlyStunned) yield break;
        CurrentlyStunned = true;
        Player.GiveEffect(Effect.Ensnared);
        Player.GiveEffect(Effect.Amnesia);
        Player.GiveEffect(Effect.Blinded);
        Player.Invisible = InvisibleMode.Alive;
        var rag = new SynapseRagdoll(Player.RoleType, DamageType.Unknown, Player.Position, Player.Rotation, Vector3.one,
            Player.NickName);
        Player.SendHint(MoreWeapons.Translation.Get(Player).Tranquilized);

        yield return Timing.WaitForSeconds(Random.Range(MoreWeapons.TzConfig.MinStunTime,
            MoreWeapons.TzConfig.MaxStunTime));

        Player.Invisible = InvisibleMode.None;
        rag.Destroy();
        Player.GiveEffect(Effect.Ensnared, 0);
        Player.GiveEffect(Effect.Amnesia, 0);
        Player.GiveEffect(Effect.Blinded, 0);
        CurrentlyStunned = false;
    }
    
    public bool In1499Dimension { get; set; }
    private Vector3 _realPosition;
    private SynapseItem _scp1499;

    public void Enter1499(SynapseItem scp1499)
    {
        if (Player.ZoneId == (uint)ZoneType.Pocket)
        {
            Player.SendHint(MoreWeapons.Translation.Get(Player).Pocket1499);
            return;
        }
        
        Player.SendHint(MoreWeapons.Translation.Get(Player).Entered1499);
        _realPosition = Player.Position;
        Player.Position = Get1499Position();
        KickOut(MoreWeapons.Scp1499Config.Scp1499ResidenceTime);
        In1499Dimension = true;

        _scp1499 = scp1499;
    }

    public void Leave1499()
    {
        if(!In1499Dimension) return;
        Player.Position = _realPosition;

        if (DecontaminationService.IsDecontaminationInProgress && Player.ZoneId == (uint)ZoneType.Lcz)
            Player.GiveEffect(Effect.Decontaminating);
        
        if (NukeService.State == NukeState.Detonated && Player.ZoneId != (uint)ZoneType.Surface)
            Player.Hurt(new WarheadDamageHandler());

        if (_scp1499 != null)
            _scp1499.ObjectData["1499"] = Time.time + MoreWeapons.Scp1499Config.Cooldown;
        _scp1499 = null;
        Timing.KillCoroutines(_coroutine.ToArray());
        In1499Dimension = false;
    }

    public Vector3 Get1499Position()
    {
        var spawn =
            MoreWeapons.EventHandlers.Scp1499Dimension.RoomSchematic.CustomObjects.FirstOrDefault(x => x.ID == 1499);
        return spawn?.Position ?? MoreWeapons.EventHandlers.Scp1499Dimension.Position;
    }
    
    public void KickOut(float delay)
    {
        if (delay < 0f) return;
        _coroutine.Add(Timing.RunCoroutine(KickOutOfScp1499(delay)));
    }

    private readonly List<CoroutineHandle> _coroutine = new List<CoroutineHandle>();

    private IEnumerator<float> KickOutOfScp1499(float delay)
    {
        yield return Timing.WaitForSeconds(delay);

        if (In1499Dimension)
            Leave1499();
    }
}