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

        private static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(StandardLevelScenesTransitionSetupDataSO), nameof(StandardLevelScenesTransitionSetupDataSO.Init),
                new[]
                {
                    typeof(string),
                    typeof(IDifficultyBeatmap),
                    typeof(IPreviewBeatmapLevel),
                    typeof(OverrideEnvironmentSettings),
                    typeof(ColorScheme),
                    typeof(ColorScheme),
                    typeof(GameplayModifiers),
                    typeof(PlayerSpecificSettings),
                    typeof(PracticeSettings),
                    typeof(string),
                    typeof(bool),
                    typeof(bool),
                    typeof(BeatmapDataCache),
                    typeof(RecordingToolManager.SetupData?)
                });

            yield return AccessTools.Method(typeof(MultiplayerLevelScenesTransitionSetupDataSO), nameof(MultiplayerLevelScenesTransitionSetupDataSO.Init),
                new[]
                {
                    typeof(string),
                    typeof(IPreviewBeatmapLevel),
                    typeof(BeatmapDifficulty),
                    typeof(BeatmapCharacteristicSO),
                    typeof(IDifficultyBeatmap),
                    typeof(ColorScheme),
                    typeof(GameplayModifiers),
                    typeof(PlayerSpecificSettings),
                    typeof(PracticeSettings),
                    typeof(bool)
                });
        }

        private static void Prefix(ref IDifficultyBeatmap difficultyBeatmap, ref ColorScheme? overrideColorScheme)
        {
            if (difficultyBeatmap == null || !PluginConfig.Instance.Enabled || PluginConfig.Instance.SelectedPresetId == null)
            {
                return;
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
                return;
            }

            var environmentInfoSO = difficultyBeatmap.GetEnvironmentInfo();
            var fallbackScheme = overrideColorScheme ?? new ColorScheme(environmentInfoSO.colorScheme);

            overrideColorScheme = selectedPreset.ToColorScheme(fallbackScheme);
            Plugin.Log.Info($"Overriding user color scheme with preset \"{selectedPreset.name}\" (ID: {selectedPreset.colorSchemeId})");
        }
    }
}
