using System;
using System.Reflection;
using BreathControl.Settings;
using BreathControl.Settings.Layouts;
using HarmonyLib;
using Sandbox.Graphics.GUI;
using VRage.Plugins;
using Sandbox.Game.Entities.Character;
using VRage.Audio;
using Sandbox.Game.Entities;

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
            
            Config.Current.PropertyChanged += Instance.CurrentConfig_PropertyChanged;
            Instance.ApplyConfig(Config.Current.BreathVolumeMult, Config.Current.BreathToggle);
        }

        // Even though it 'applies' the config to the current session, it does not save unless the user hits 'apply' on the plugin menu.

        private void CurrentConfig_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ApplyConfig(Config.Current.BreathVolumeMult, Config.Current.BreathToggle);   
        }

        public void Dispose()
        {
            Instance = null;
        }

        private static bool isinit_breath = false;
        public void UpdateBreathSounds(MyCharacterBreath _this)
        {
            IMySourceVoice voice = (IMySourceVoice)typeof(MyCharacterBreath).GetField("m_sound", BindingFlags.NonPublic | BindingFlags.Instance).GetValueDirect(__makeref(_this));
            
            if (voice != null){
                voice.VolumeMultiplier = breathMultiplier * (enableBreath ? 1 : 0);
            }
            else if(voice == null && Sandbox.Game.World.MySession.Static.Ready && !isinit_breath) {
                isinit_breath = true;

                MyCharacter character = (MyCharacter)typeof(MyCharacterBreath).GetField("m_character", BindingFlags.NonPublic | BindingFlags.Instance).GetValueDirect(__makeref(_this));
                MySoundPair m_breathHeavy = new MySoundPair(string.IsNullOrEmpty(character.Definition.BreathHeavySoundName) ? BREATH_HEAVY : character.Definition.BreathHeavySoundName);
                var PlaySndMethod = typeof(MyCharacterBreath).GetMethod("PlaySound", BindingFlags.NonPublic | BindingFlags.Instance);

                PlaySndMethod.Invoke(_this, new object[] { m_breathHeavy.SoundId, true });

                voice = (IMySourceVoice)typeof(MyCharacterBreath).GetField("m_sound", BindingFlags.NonPublic | BindingFlags.Instance).GetValueDirect(__makeref(_this));
                if (voice != null){
                    voice.VolumeMultiplier = breathMultiplier * (enableBreath ? 1 : 0);
                }
            }
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
           // required interface method
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