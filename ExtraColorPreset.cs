using BeatSaberMarkupLanguage.Attributes;
using BSExtraColorPresets.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BSExtraColorPresets
{
    public class ExtraColorPreset
    {
        public static string DEFAULT_PRESET_NAME = "0";
        public static Color DEFAULT_SABER_A_COLOR = new Color(RgbToFloat(168f), RgbToFloat(32f), RgbToFloat(32f));
        public static Color DEFAULT_SABER_B_COLOR = new Color(RgbToFloat(32f), RgbToFloat(100f), RgbToFloat(168f));
        public static Color DEFAULT_ENV_0_COLOR = new Color(RgbToFloat(192f), RgbToFloat(48f), RgbToFloat(48f));
        public static Color DEFAULT_ENV_1_COLOR = new Color(RgbToFloat(48f), RgbToFloat(152f), RgbToFloat(255f));
        public static Color DEFAULT_OBSTACLES_COLOR = new Color(RgbToFloat(255f), RgbToFloat(48f), RgbToFloat(48f));
        public static Color DEFAULT_ENV_0_BOOST_COLOR = new Color(0, 0, 0, 0);
        public static Color DEFAULT_ENV_1_BOOST_COLOR = new Color(0, 0, 0, 0);

        public virtual string colorSchemeId { get; set; } = GenerateUniqueId();

        [UIValue("preset-name")]
        public virtual string name { get; set; } = DEFAULT_PRESET_NAME;

        [UIValue("saber-a-color")]
        public virtual Color saberAColor { get; set; } = DEFAULT_SABER_A_COLOR;

        [UIValue("saber-b-color")]
        public virtual Color saberBColor { get; set; } = DEFAULT_SABER_B_COLOR;

        [UIValue("env-0-color")]
        public virtual Color environmentColor0 { get; set; } = DEFAULT_ENV_0_COLOR;

        [UIValue("env-1-color")]
        public virtual Color environmentColor1 { get; set; } = DEFAULT_ENV_1_COLOR;

        [UIValue("obstacles-color")]
        public virtual Color obstaclesColor { get; set; } = DEFAULT_OBSTACLES_COLOR;

        public virtual Color environmentColor0Boost { get; set; } = DEFAULT_ENV_0_BOOST_COLOR;
        public virtual Color environmentColor1Boost { get; set; } = DEFAULT_ENV_1_BOOST_COLOR;

        public static string GenerateUniqueId()
        {
            return $"ExtraColorScheme{Guid.NewGuid()}";
        }

        internal static float RgbToFloat(float value) {
            return value / 255f;
        }

        public ColorScheme ToColorScheme(ColorScheme fallbackScheme)
        {
            return new ColorScheme(colorSchemeId, "", true, name, false, saberAColor, saberBColor, environmentColor0, environmentColor1, fallbackScheme.environmentColorW, true, environmentColor0Boost, environmentColor1Boost, fallbackScheme.environmentColorWBoost, obstaclesColor);
        }

        [UIAction("reload-data")]
        public void ReloadData(string value)
        {
            Plugin.Log.Info("ExtraColorPreset.ReloadData "+value);
            Settings.instance.UpdateSelectedDropdownOptions();
        }

    }

}
