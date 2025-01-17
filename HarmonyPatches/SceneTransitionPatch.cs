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
        private static string previousMapID;
        private static string previousSchemeID;
        
        [HarmonyPatch]
        internal class CustomSongColorsPatch
        {
            [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), "InitColorInfo")]
            internal class StandardLevelScenesTransitionSetupDataPatch
            {
                private static void Postfix(StandardLevelScenesTransitionSetupDataSO __instance)
                {
                    var overrideScheme = GetOverrideColorScheme(__instance.colorScheme, __instance.beatmapLevel);
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
                    var overrideScheme = GetOverrideColorScheme(__instance.colorScheme, __instance.beatmapLevel);
                    if (overrideScheme == null) return;

                    __instance.usingOverrideColorScheme = true;
                    __instance.colorScheme = overrideScheme;
                }
            }
        }

        private static ColorScheme? GetOverrideColorScheme(ColorScheme fallbackScheme, BeatmapLevel mapData)
        {
            if (!PluginConfig.Instance.Enabled || PluginConfig.Instance.SelectedPresetId == null)
            {
                return null;
            }

            ExtraColorPresetV2 selectedPreset;
            
            if (PluginConfig.Instance.SelectedPresetId == MinimalExtraColorPreset.randomItem.colorSchemeId)
            {
                Plugin.Log.Info($"Preset selection set to random, picking from available presets…");
                string selectedPresetID = null;
                
                string mapID = mapData.levelID;
                if (mapID == previousMapID)
                {
                    Plugin.Log.Info("Map is the same as the previous map, using previously selected preset");
                    selectedPresetID = previousSchemeID;
                }
                
                selectedPreset = PluginConfig.Instance.ExtraColorPresetsV2.Find(preset => preset.colorSchemeId == selectedPresetID);
                if (selectedPreset == null)
                {
                    Plugin.Log.Info("Selected preset was null, fetching a new one…");
                    var randomPresetIndex = random.Next(PluginConfig.Instance.ExtraColorPresetsV2.Count());
                    selectedPreset = PluginConfig.Instance.ExtraColorPresetsV2[randomPresetIndex];
                    selectedPresetID = PluginConfig.Instance.ExtraColorPresetsV2[randomPresetIndex].colorSchemeId;
                }
                
                previousMapID = mapID;
                previousSchemeID = selectedPresetID;
            }
            else if (PluginConfig.Instance.SelectedPresetId == MinimalExtraColorPreset.randomUniqueItem.colorSchemeId)
            {
                Plugin.Log.Info($"Preset selection set to uniquely random, picking from available presets…");
                string selectedPresetID = null;
                
                string mapID = mapData.levelID;
                if (mapID == previousMapID)
                {
                    Plugin.Log.Info("Map is the same as the previous map, using previously selected preset");
                    selectedPresetID = previousSchemeID;
                }
                
                selectedPreset = PluginConfig.Instance.ExtraColorPresetsV2.Find(preset => preset.colorSchemeId == selectedPresetID);

                if (selectedPreset == null)
                {
                    Plugin.Log.Info("Selected preset was null, fetching a new one…");
                    var availablePresets = PluginConfig.Instance.ExtraColorPresetsV2.FindAll(preset => !Plugin.ExtraColorPresetsRandomlySelectedIds.Contains(preset.colorSchemeId));
                    Plugin.Log.Info($"{availablePresets} preset(s) available");
                    if (!availablePresets.Any())
                    {
                        Plugin.Log.Info("Ran out of selectable presets, reinitializing blocklist…");
                        Plugin.ReinitUniqueSelectables();
                    }

                    // Proactively prevent selecting the previous scheme when possible for more variety
                    if (availablePresets.Count > 1 && previousSchemeID != null)
                    {
                        availablePresets = availablePresets.Where(preset => preset.colorSchemeId != previousSchemeID).ToList();
                    }

                    var randomPresetIndex = random.Next(availablePresets.Count);
                    selectedPresetID = availablePresets.ElementAt(randomPresetIndex).colorSchemeId;
                    Plugin.ExtraColorPresetsRandomlySelectedIds.Add(selectedPresetID);
                    selectedPreset = PluginConfig.Instance.ExtraColorPresetsV2.Find(preset => preset.colorSchemeId == selectedPresetID);
                }

                previousMapID = mapID;
                previousSchemeID = selectedPresetID;
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