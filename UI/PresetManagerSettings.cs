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

namespace BSExtraColorPresets.UI
{
    public class PresetManagerSettings : MonoBehaviour
    {
        public static PresetManagerSettings Instance = new PresetManagerSettings();

        public void Initialize()
        {
            Plugin.Log.Debug("Adding mod settings menu");
            BSMLSettings.Instance.AddSettingsMenu("Extra Color Presets", "BSExtraColorPresets.UI.PresetManagerViewController.bsml", this);
        }

        [UIComponent("preset-list-display")]
        public CustomCellListTableData? presetListDisplay;

        [UIComponent("preset-settings-modal")]
        public ModalViewBase presetSettingsModal;

        [UIComponent("preset-settings-modal-title")]
        public TextMeshProUGUI presetSettingsModalTitle;

        [UIComponent("preset-settings-name")]
        public StringSetting presetSettingsName;

        [UIComponent("preset-settings-left-saber-color")]
        public ColorSetting presetSettingsLeftSaberColor;

        [UIComponent("preset-settings-right-saber-color")]
        public ColorSetting presetSettingsRightSaberColor;

        [UIComponent("preset-settings-env-0-color")]
        public ColorSetting presetSettingsEnv0Color;

        [UIComponent("preset-settings-env-1-color")]
        public ColorSetting presetSettingsEnv1Color;

        [UIComponent("preset-settings-obstacles-color")]
        public ColorSetting presetSettingsObstaclesColor;

        [UIComponent("preset-settings-boost-0-color")]
        public ColorSetting presetSettingsBoost0Color;

        [UIComponent("preset-settings-boost-1-color")]
        public ColorSetting presetSettingsBoost1Color;

        [UIComponent("preset-delete-modal")]
        public ModalViewBase presetDeleteModal;

        [UIComponent("preset-delete-modal-title")]
        public TextMeshProUGUI presetDeleteModalTitle;

        [UIComponent("mode-switch")]
        public Button? modeSwitchButton;

        protected ExtraColorPresetV2? modalEditingPreset = null;
        protected bool deleteMode = false;

        [UIValue("preset-object-list")]
        public List<ExtraColorPresetV2> presetObjectsList => PluginConfig.Instance.ExtraColorPresetsV2;

        [UIAction("#post-parse")]
        public void UpdatePresetList()
        {
            presetListDisplay?.TableView.ReloadDataKeepingPosition();
            PresetSelectorSettings.Instance.UpdatePresetList();
        }

        [UIAction("click-add-preset-action")]
        private void ClickAddPresetAction()
        {
            var newPreset = new ExtraColorPresetV2();
            newPreset.name = ExtraColorPresetV2.GenerateName(PluginConfig.Instance.ExtraColorPresetsV2.Count());
            PluginConfig.Instance.ExtraColorPresetsV2.Add(newPreset);
            Plugin.ExtraColorPresetsUniqueSelectable.Add(newPreset.colorSchemeId);
            UpdatePresetList();
        }

        public void EditPreset(ExtraColorPresetV2 preset)
        {
            modalEditingPreset = preset;

            presetSettingsModalTitle.text = $"Editing preset \"{modalEditingPreset.name}\"";
            presetSettingsName.Text = modalEditingPreset.name;
            presetSettingsLeftSaberColor.CurrentColor = modalEditingPreset.saberAColor.ToUnityColor();
            presetSettingsRightSaberColor.CurrentColor = modalEditingPreset.saberBColor.ToUnityColor();
            presetSettingsEnv0Color.CurrentColor = modalEditingPreset.environmentColor0.ToUnityColor();
            presetSettingsEnv1Color.CurrentColor = modalEditingPreset.environmentColor1.ToUnityColor();
            presetSettingsObstaclesColor.CurrentColor = modalEditingPreset.obstaclesColor.ToUnityColor();
            presetSettingsBoost0Color.CurrentColor = modalEditingPreset.environmentColor0Boost.ToUnityColor();
            presetSettingsBoost1Color.CurrentColor = modalEditingPreset.environmentColor1Boost.ToUnityColor();

            presetSettingsModal.Show(true);
        }

        public void ConfirmDeletePreset(ExtraColorPresetV2 preset)
        {
            modalEditingPreset = preset;

            presetDeleteModalTitle.text = $"Are you sure you want to delete the preset \"{modalEditingPreset.name}\"?";
            presetDeleteModal.Show(true);
        }

        [UIAction("save-preset-settings-action")]
        private void SavePresetSettingsAction()
        {
            if (modalEditingPreset != null)
            {
                modalEditingPreset.name = presetSettingsName.Text;
                modalEditingPreset.saberAColor = new Util.HexColor(presetSettingsLeftSaberColor.CurrentColor);
                modalEditingPreset.saberBColor = new Util.HexColor(presetSettingsRightSaberColor.CurrentColor);
                modalEditingPreset.environmentColor0 = new Util.HexColor(presetSettingsEnv0Color.CurrentColor);
                modalEditingPreset.environmentColor1 = new Util.HexColor(presetSettingsEnv1Color.CurrentColor);
                modalEditingPreset.obstaclesColor = new Util.HexColor(presetSettingsObstaclesColor.CurrentColor);
                modalEditingPreset.environmentColor0Boost = new Util.HexColor(presetSettingsBoost0Color.CurrentColor);
                modalEditingPreset.environmentColor1Boost = new Util.HexColor(presetSettingsBoost1Color.CurrentColor);
            }
            ClosePresetSettingsAction();
        }

        [UIAction("close-preset-settings-action")]
        private void ClosePresetSettingsAction()
        {
            modalEditingPreset = null;
            presetSettingsModal.Hide(true);
            UpdatePresetList();
        }

        [UIAction("delete-preset-action")]
        private void DeletePresetAction()
        {
            if (modalEditingPreset != null)
            {
                if (Plugin.ExtraColorPresetsUniqueSelectable.Contains(modalEditingPreset.colorSchemeId))
                {
                    Plugin.ExtraColorPresetsUniqueSelectable.Remove(modalEditingPreset.colorSchemeId);   
                }
                PluginConfig.Instance.ExtraColorPresetsV2.Remove(modalEditingPreset);
            }
            ClosePresetDeleteAction();
        }

        [UIAction("close-preset-delete-action")]
        private void ClosePresetDeleteAction()
        {
            modalEditingPreset = null;
            presetDeleteModal.Hide(true);
            UpdatePresetList();
        }
    }
}
