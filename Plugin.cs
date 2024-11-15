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
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace BSExtraColorPresets
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {

        public const string PluginName = "extracolorpresets";
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        private static readonly HarmonyLib.Harmony Harmony = new HarmonyLib.Harmony($"art.djdavid98.{PluginName}");

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
            try { Harmony.PatchAll(Assembly.GetExecutingAssembly()); }
            catch (Exception ex) { Log.Debug(ex); }
            Log.Info("BSExtraColorPresets initialized.");
        }

        private void LoadConfig(Config conf)
        {
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            Log.Debug("Config loaded");
            if (!PluginConfig.Instance.ExtraColorPresetsV2.Any())
            {
                Log.Debug("Adding initial blank preset");
                PluginConfig.Instance.ExtraColorPresetsV2.Add(new ExtraColorPresetV2());
            }
        }

        [OnStart]
        public async Task OnApplicationStart()
        {
            new GameObject("BSExtraColorPresetsController").AddComponent<BSExtraColorPresetsController>();

            await MainMenuAwaiter.WaitForMainMenuAsync();
            MainMenuAwaiter.MainMenuInitializing += ReinitSettings;
            ReinitSettings();
        }

        private void ReinitSettings()
        {
            PresetManagerSettings.Instance.Initialize();
            PresetSelectorSettings.Instance.Initialize();
        }

        [OnExit]
        public void OnApplicationQuit()
        {
        }
    }
}
