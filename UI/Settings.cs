using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
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

namespace BSExtraColorPresets.UI
{
    public class Settings : MonoBehaviour
    {
        public static Settings instance = new Settings();

        [UIValue("enable-plugin")]
        public bool enablePlugin { get { return PluginConfig.Instance.Enabled; } set { PluginConfig.Instance.Enabled = value; } }

        [UIValue("selected-preset")]
        public ExtraColorPreset selectedPreset {
            get {
                var selectedPreset = PluginConfig.Instance.ExtraColorPresets.Find(preset => preset.colorSchemeId == PluginConfig.Instance.SelectedPresetId);
                return selectedPreset;
            }
            set { PluginConfig.Instance.SelectedPresetId = value.colorSchemeId; }
        }

        [UIAction("preset-name")]
        public string PresetNameFormatter(ExtraColorPreset preset)
        {
            return preset.name;
        }

        [UIComponent("selected-preset-dd")]
        public DropDownListSetting selectedPresetDd;

        [UIComponent("preset-list")]
        public CustomCellListTableData presetList;

        [UIValue("presets")]
        public List<ExtraColorPreset> presetObjectsList => PluginConfig.Instance.ExtraColorPresets;

        [UIAction("#post-parse")]
        public void UpdatePresetList()
        {
            presetList.tableView.ReloadData();
            UpdateSelectedDropdownOptions();
        }


        [UIAction("click-add-preset-action")]
        private void ClickAddPresetAction() {
            Plugin.Log.Info("ClickAddPresetAction");
            var newPreset = new ExtraColorPreset();
            newPreset.name = PluginConfig.Instance.ExtraColorPresets.Count().ToString();
            PluginConfig.Instance.ExtraColorPresets.Add(newPreset);
            UpdatePresetList();
            UpdateSelectedDropdownOptions();
        }

        public void UpdateSelectedDropdownOptions()
        {
            selectedPresetDd.values = PluginConfig.Instance.ExtraColorPresets;
            selectedPresetDd.UpdateChoices();
        }
    }
}
