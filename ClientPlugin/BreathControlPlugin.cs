using System;
using System.Reflection;
using BreathControl.Settings;
using BreathControl.Settings.Layouts;
using HarmonyLib;
using Sandbox.Graphics.GUI;
using VRage.Plugins;
using Sandbox.Game.World;
using VRage.Data.Audio;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities;
using VRage.Audio;
using VRage.Utils;
using System.Text;
using VRageMath;
using VRage.Game;
using VRage;
using Sandbox.Definitions;
using Sandbox;

namespace BreathControl
{

    // ReSharper disable once UnusedType.Global
    public class BreathControlPlugin : IPlugin, IDisposable
    {
        //private readonly string BREATH_CALM = "PlayVocBreath1L";
        private readonly string BREATH_HEAVY = "PlayVocBreath2L";
       /* private readonly string OXYGEN_CHOKE_NORMAL = "PlayChokeA";
        private readonly string OXYGEN_CHOKE_LOW = "PlayChokeB";
        private readonly string OXYGEN_CHOKE_CRITICAL = "PlayChokeC";*/
        public const string Name = "BreathControl";
        public static BreathControlPlugin Instance { get; private set; }
        public static float breathMultiplier = 1.0F;
        public static Boolean enableBreath = true;
        private SettingsGenerator settingsGenerator;
        

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Instance = this;
            Instance.settingsGenerator = new SettingsGenerator();

            
            // TODO: Put your one time initialization code here.
            Harmony harmony = new Harmony(Name);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
   
            Config.Current.PropertyChanged += Instance.Current_PropertyChanged;
        }

        private void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ApplyConfig(Config.Current.BreathVolumeMult, Config.Current.BreathToggle);   
        }

        public void Dispose()
        {
            Instance = null;
        }

        public void UpdateBreathSounds(MyCharacterBreath _this)
        {
            if (!MySandboxGame.IsGameReady || _this == null)
            {
                return;
            }

            IMySourceVoice voice = (IMySourceVoice)typeof(MyCharacterBreath).GetField("m_sound", BindingFlags.NonPublic | BindingFlags.Instance).GetValueDirect(__makeref(_this));
            if (voice != null)
            {
                voice.VolumeMultiplier = BreathControlPlugin.breathMultiplier * (BreathControlPlugin.enableBreath ? 1 : 0);
            }
        }

        private static void PlaySound(MyCueId soundId, bool useCrossfade)
        {
           MyAudio.Static.PlaySound(soundId);
        }

        // ReSharper disable once UnusedMember.Global
        public void OpenConfigDialog()
        {           
            Instance.settingsGenerator.SetLayout<Simple>();
            MyGuiSandbox.AddScreen(Instance.settingsGenerator.Dialog);
            
        }

        public void ApplyConfig(float breathMultiplier = 1.0F, bool enableBreath=true){           
            BreathControlPlugin.breathMultiplier = breathMultiplier;
            BreathControlPlugin.enableBreath = enableBreath;            
        }

        public void Update()
        {
           // required
        }
    }

    [HarmonyPatch(typeof(MyCharacterBreath), "Update")]
    public class MyCharacterBreathUpdatePatch
    {
        public static void Prefix(MyCharacterBreath __instance)
        {
            BreathControlPlugin.Instance.UpdateBreathSounds(__instance);
        }
    }
}