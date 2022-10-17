using MoreWeapons.Configs;
using Neuron.Core.Plugins;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Events;

namespace MoreWeapons;

[Plugin(
    Name = "MoreWeapons",
    Author = "Dimenzio",
    Description = "Adds many new Weapons to the Game",
    Version = "1.0.0"
    )]
public class MoreWeapons : ReloadablePlugin
{
    public GrenadeLauncherConfig GLConfig { get; private set; }
    public Scp127Config Scp127Config { get; private set; }
    public SniperConfig SnConfig { get; private set; }
    public TranquilizerConfig TzConfig{ get; private set; }
    public Scp1499Config Scp1499Config { get; private set; }
    public MedkitGunConfig MedkitGunConfig{ get; private set; }
    public VaccinePistolConfig VPConfig { get; private set; }
    public XlMedkitConfig XlMedkitConfig { get; private set; }
    public MoreWeaponsTranslation Translation{ get; private set; }
    public EventHandlers EventHandlers { get; private set; }
    
    public override void EnablePlugin()
    {
        Logger.Info("More Weapons loaded");
        EventHandlers = Synapse.GetEventHandler<EventHandlers>();
    }

    public override void Reload(ReloadEvent _ = null)
    {
        GLConfig = Synapse.Get<GrenadeLauncherConfig>();
        Scp127Config = Synapse.Get<Scp127Config>();
        SnConfig = Synapse.Get<SniperConfig>();
        TzConfig = Synapse.Get<TranquilizerConfig>();
        Scp1499Config = Synapse.Get<Scp1499Config>();
        MedkitGunConfig = Synapse.Get<MedkitGunConfig>();
        VPConfig = Synapse.Get<VaccinePistolConfig>();
        XlMedkitConfig = Synapse.Get<XlMedkitConfig>();
        Translation = Synapse.Get<MoreWeaponsTranslation>();
    }
}