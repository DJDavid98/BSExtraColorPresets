using BeatSaberMarkupLanguage.Settings;
using BSExtraColorPresets.Configuration;
using BSExtraColorPresets.UI;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace BSExtraColorPresets
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {

        public const string PLUGIN_NAME = "extracolorpresets";
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        internal static readonly HarmonyLib.Harmony harmony = new HarmonyLib.Harmony($"art.djdavid98.{PLUGIN_NAME}");

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger, Config conf)
        {
            Instance = this;
            Log = logger;
            LoadConfig(conf);
            Log.Debug("Apply Harmony patches");
            try { harmony.PatchAll(Assembly.GetExecutingAssembly()); }
            catch (Exception ex) { Log.Debug(ex); }
            Log.Info("BSExtraColorPresets initialized.");
        }

        private void LoadConfig(Config conf)
        {
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            Log.Debug("Config loaded");
#pragma warning disable CS0618 // Type or member is obsolete
            if (PluginConfig.Instance.ExtraColorPresets != null && PluginConfig.Instance.ExtraColorPresets.Count() > 0)
            {
                Log.Debug("Migrating old color presets…");
                PluginConfig.Instance.ExtraColorPresetsV2 = PluginConfig.Instance.ExtraColorPresets.ConvertAll(preset => preset.ToV2());
                PluginConfig.Instance.ExtraColorPresets = null;
                Log.Debug("Migrated old color presets");
            }
#pragma warning restore CS0618 // Type or member is obsolete
            if (PluginConfig.Instance.ExtraColorPresetsV2.Count() == 0)
            {
                Log.Debug("Adding initial blank preset");
                PluginConfig.Instance.ExtraColorPresetsV2.Add(new ExtraColorPresetV2());
            }

            PresetManagerSettings.Instance.Initialize();
            PresetSelectorSettings.Instance.Initialize();
        }

        [OnStart]
        public void OnApplicationStart()
        {
            new GameObject("BSExtraColorPresetsController").AddComponent<BSExtraColorPresetsController>();
        }

        [OnExit]
        public void OnApplicationQuit()
        {
        }
    }
}
