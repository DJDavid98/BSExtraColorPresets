using BSExtraColorPresets.Configuration;
using BSExtraColorPresets.UI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BSExtraColorPresets.HarmonyPatches
{
    [HarmonyPatch]
    internal class SceneTransitionPatch
    {
        private static Random random = new Random();
        [HarmonyPatch]
        internal class CustomSongColorsPatch
        {
            [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), "InitColorInfo")]
            internal class StandardLevelScenesTransitionSetupDataPatch
            {
                private static void Postfix(StandardLevelScenesTransitionSetupDataSO __instance)
                {
                    var overrideScheme = GetOverrideColorScheme(__instance.colorScheme);
                    if (overrideScheme == null) return;

                    __instance.usingOverrideColorScheme = true;
                    __instance.colorScheme = overrideScheme;
                }
            }

            [HarmonyPatch(typeof(MultiplayerLevelScenesTransitionSetupDataSO), "InitColorInfo")]
            internal class MultiplayerLevelScenesTransitionSetupDataPatch
            {
                private static void Postfix(MultiplayerLevelScenesTransitionSetupDataSO __instance)
                {
                    var overrideScheme = GetOverrideColorScheme(__instance.colorScheme);
                    if (overrideScheme == null) return;

                    __instance.usingOverrideColorScheme = true;
                    __instance.colorScheme = overrideScheme;
                }
            }
        }

        private static ColorScheme? GetOverrideColorScheme(ColorScheme fallbackScheme)
        {
            if (!PluginConfig.Instance.Enabled || PluginConfig.Instance.SelectedPresetId == null)
            {
                return null;
            }

            ExtraColorPresetV2 selectedPreset;
            if (PluginConfig.Instance.SelectedPresetId == MinimalExtraColorPreset.randomItem.colorSchemeId)
            {
                Plugin.Log.Info($"Preset selection set to random, picking from available presets…");
                var randomPresetIndex = random.Next(PluginConfig.Instance.ExtraColorPresetsV2.Count());
                selectedPreset = PluginConfig.Instance.ExtraColorPresetsV2[randomPresetIndex];
            }
            else
            {
                selectedPreset = PluginConfig.Instance.ExtraColorPresetsV2.Find(preset => preset.colorSchemeId == PluginConfig.Instance.SelectedPresetId);
            }
            if (selectedPreset == null)
            {
                return null;
            }

            var overrideColorScheme = selectedPreset.ToColorScheme(fallbackScheme);
            Plugin.Log.Info($"Overriding user color scheme with preset \"{selectedPreset.name}\" (ID: {selectedPreset.colorSchemeId})");
            return overrideColorScheme;
        }
    }
}
