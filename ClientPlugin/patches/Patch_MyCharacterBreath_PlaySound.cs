using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreathControl.patches
{
    using HarmonyLib;
    using Sandbox.Game.Entities.Character;
    using VRage.Audio;
    using VRage.Data.Audio;

    namespace ClientPlugin.patches
    {
        [HarmonyPatch(typeof(MyCharacterBreath), "PlaySound")]
        public static class Patch_MyCharacterBreath_PlaySound
        {
            static readonly MyCueId BREATH_HEAVY_ID = MyAudio.Static.GetCueId("PlayVocBreath2L");

            static void Prefix(MyCharacterBreath __instance, MyCueId soundId)
            {              

                if (soundId == BREATH_HEAVY_ID)
                {
                    var voiceField = AccessTools.Field(typeof(MyCharacterBreath), "m_sound");
                    if (voiceField != null)
                    {
                        var voice = voiceField.GetValue(__instance) as IMySourceVoice;
                        if (voice != null)
                        {
                            voice.VolumeMultiplier = BreathControlPlugin.breathMultiplier * (BreathControlPlugin.enableBreath ? 1f : 0f);                       
                        }
                    }
                }
            }
        }
    }

}
