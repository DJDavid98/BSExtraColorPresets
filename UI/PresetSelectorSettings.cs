using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.Settings;
using BSExtraColorPresets.Configuration;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SliderController.Pool;

namespace BSExtraColorPresets.UI
{
    public class PresetSelectorSettings : MonoBehaviour
    {
        public static PresetSelectorSettings Instance = new PresetSelectorSettings();

        public void Initialize()
        {
            Plugin.Log.Debug("Adding multiplayer settings menu");
            GameplaySetup.instance.AddTab("Extra Color Presets", "BSExtraColorPresets.UI.PresetSelectorViewController.bsml", this, MenuType.All);
        }

        [UIValue("enable-plugin")]
        public bool enablePlugin { get { return PluginConfig.Instance.Enabled; } set { PluginConfig.Instance.Enabled = value; } }

        [UIValue("selected-preset")]
        public ExtraColorPresetV2 selectedPreset
        {
            get
            {
                var selectedPreset = PluginConfig.Instance.ExtraColorPresetsV2.Find(preset => preset.colorSchemeId == PluginConfig.Instance.SelectedPresetId);
                return selectedPreset;
            }
            set { PluginConfig.Instance.SelectedPresetId = value.colorSchemeId; }
        }

        [UIAction("preset-name")]
        public string PresetNameFormatter(ExtraColorPresetV2 preset)
        {
            return preset.name;
        }

        [UIComponent("selected-preset-dd")]
        public DropDownListSetting selectedPresetDd;

        [UIValue("presets")]
        public List<ExtraColorPresetV2> presetObjectsList => PluginConfig.Instance.ExtraColorPresetsV2;

        [UIAction("#post-parse")]
        public void UpdatePresetList()
        {
            UpdateSelectedDropdownOptions();
        }

        public void UpdateSelectedDropdownOptions()
        {
            if (selectedPresetDd == null) { return; }
            selectedPresetDd.values = PluginConfig.Instance.ExtraColorPresetsV2;
            selectedPresetDd.UpdateChoices();
        }
    }
}
