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
            Log.Info("BSExtraColorPresets initialized.");
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            Log.Debug("Config loaded");
            Log.Debug("Apply Harmony patches");
            try { harmony.PatchAll(Assembly.GetExecutingAssembly()); }
            catch (Exception ex) { Log.Debug(ex); }
            Log.Debug("Adding settings menu");
            BSMLSettings.instance.AddSettingsMenu("Extra Color Presets", "BSExtraColorPresets.UI.SettingsViewController.bsml", Settings.instance);
        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
        }
        */
        #endregion

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Debug("OnApplicationStart");
            new GameObject("BSExtraColorPresetsController").AddComponent<BSExtraColorPresetsController>();

        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");

        }
    }
}
