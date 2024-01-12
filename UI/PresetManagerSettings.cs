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
    public class PresetManagerSettings : MonoBehaviour
    {
        public static PresetManagerSettings Instance = new PresetManagerSettings();

        public void Initialize()
        {
            Plugin.Log.Debug("Adding mod settings menu");
            BSMLSettings.instance.AddSettingsMenu("Extra Color Presets", "BSExtraColorPresets.UI.PresetManagerViewController.bsml", this);
        }

        [UIComponent("preset-list")]
        public CustomCellListTableData? presetList;

        [UIComponent("preset-settings-modal")]
        public ModalView presetSettingsModal;

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

        [UIComponent("preset-delete-modal")]
        public ModalView presetDeleteModal;

        [UIComponent("preset-delete-modal-title")]
        public TextMeshProUGUI presetDeleteModalTitle;

        [UIComponent("mode-switch")]
        public Button? modeSwitchButton;

        protected ExtraColorPresetV2? modalEditingPreset = null;
        protected bool deleteMode = false;

        [UIValue("presets")]
        public List<ExtraColorPresetV2> presetObjectsList => PluginConfig.Instance.ExtraColorPresetsV2;

        [UIAction("#post-parse")]
        public void UpdatePresetList()
        {
            presetList?.tableView.ReloadData();
            UpdateDeleteModeButtonText();
        }

        [UIAction("click-add-preset-action")]
        private void ClickAddPresetAction()
        {
            Plugin.Log.Debug("ClickAddPresetAction");
            var newPreset = new ExtraColorPresetV2();
            newPreset.name = ExtraColorPresetV2.GenerateName(PluginConfig.Instance.ExtraColorPresetsV2.Count());
            PluginConfig.Instance.ExtraColorPresetsV2.Add(newPreset);
            UpdatePresetList();
        }

        [UIAction("change-mode")]
        private void ChangeModeAction()
        {
            Plugin.Log.Debug("ChangeModeAction");
            deleteMode = !deleteMode;
            UpdateDeleteModeButtonText();
        }

        public void EditPreset(ExtraColorPresetV2 preset)
        {
            Plugin.Log.Debug("EditPreset");
            modalEditingPreset = preset;

            presetSettingsModalTitle.text = $"Editing preset \"{modalEditingPreset.name}\"";
            presetSettingsName.Text = modalEditingPreset.name;
            presetSettingsLeftSaberColor.CurrentColor = modalEditingPreset.saberAColor.ToUnityColor();
            presetSettingsRightSaberColor.CurrentColor = modalEditingPreset.saberBColor.ToUnityColor();
            presetSettingsEnv0Color.CurrentColor = modalEditingPreset.environmentColor0.ToUnityColor();
            presetSettingsEnv1Color.CurrentColor = modalEditingPreset.environmentColor1.ToUnityColor();
            presetSettingsObstaclesColor.CurrentColor = modalEditingPreset.obstaclesColor.ToUnityColor();

            presetSettingsModal.Show(true);
        }
        
        public void ConfirmDeletePreset(ExtraColorPresetV2 preset)
        {
            Plugin.Log.Debug("EditPreset");
            modalEditingPreset = preset;

            presetDeleteModalTitle.text = $"Are you sure you want to delete the preset \"{modalEditingPreset.name}\"?";
            presetDeleteModal.Show(true);
        }

        [UIAction("save-preset-settings-action")]
        private void SavePresetSettingsAction()
        {
            Plugin.Log.Debug("SavePresetSettingsAction");
            if (modalEditingPreset != null)
            {
                modalEditingPreset.name = presetSettingsName.Text;
                modalEditingPreset.saberAColor = new Util.HexColor(presetSettingsLeftSaberColor.CurrentColor);
                modalEditingPreset.saberBColor = new Util.HexColor(presetSettingsRightSaberColor.CurrentColor);
                modalEditingPreset.environmentColor0 = new Util.HexColor(presetSettingsEnv0Color.CurrentColor);
                modalEditingPreset.environmentColor1 = new Util.HexColor(presetSettingsEnv1Color.CurrentColor);
                modalEditingPreset.obstaclesColor = new Util.HexColor(presetSettingsObstaclesColor.CurrentColor);
            }
            ClosePresetSettingsAction();
        }

        [UIAction("close-preset-settings-action")]
        private void ClosePresetSettingsAction()
        {
            Plugin.Log.Debug("ClosePresetSettingsAction");
            modalEditingPreset = null;
            presetSettingsModal.Hide(true);
        }

        [UIAction("delete-preset-action")]
        private void DeletePresetAction()
        {
            Plugin.Log.Debug("DeletePresetAction");
            if (modalEditingPreset != null)
            {
                PluginConfig.Instance.ExtraColorPresetsV2.Remove(modalEditingPreset);
            }
            ClosePresetDeleteAction();
        }

        [UIAction("close-preset-delete-action")]
        private void ClosePresetDeleteAction()
        {
            Plugin.Log.Debug("ClosePresetDeleteAction");
            modalEditingPreset = null;
            presetDeleteModal.Hide(true);
        }

        public void UpdateDeleteModeButtonText()
        {
            modeSwitchButton?.SetButtonText(deleteMode ? "Delete mode" : "Edit mode");
        }
    }
}
