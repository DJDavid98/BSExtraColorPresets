using BeatSaberMarkupLanguage.Attributes;
using BSExtraColorPresets.UI;
using BSExtraColorPresets.Util;
using System;
using UnityEngine;

namespace BSExtraColorPresets
{
    [Obsolete("This is deprecated, please use ExtraColorPresetV2 instead.")]
    public class ExtraColorPreset
    {
        public static string DEFAULT_PRESET_NAME = "0";
        public static Color DEFAULT_SABER_A_COLOR = new Color(Converter.RgbToFloat(168f), Converter.RgbToFloat(32f), Converter.RgbToFloat(32f));
        public static Color DEFAULT_SABER_B_COLOR = new Color(Converter.RgbToFloat(32f), Converter.RgbToFloat(100f), Converter.RgbToFloat(168f));
        public static Color DEFAULT_ENV_0_COLOR = new Color(Converter.RgbToFloat(192f), Converter.RgbToFloat(48f), Converter.RgbToFloat(48f));
        public static Color DEFAULT_ENV_1_COLOR = new Color(Converter.RgbToFloat(48f), Converter.RgbToFloat(152f), Converter.RgbToFloat(255f));
        public static Color DEFAULT_OBSTACLES_COLOR = new Color(Converter.RgbToFloat(255f), Converter.RgbToFloat(48f), Converter.RgbToFloat(48f));
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

        public ColorScheme ToColorScheme(ColorScheme fallbackScheme)
        {
            return new ColorScheme(colorSchemeId, "", true, name, false, saberAColor, saberBColor, environmentColor0, environmentColor1, true, environmentColor0Boost, environmentColor1Boost, obstaclesColor);
        }

        public ExtraColorPresetV2 ToV2()
        {
            ExtraColorPresetV2 instance = new ExtraColorPresetV2();
            instance.colorSchemeId = colorSchemeId;
            instance.name = name;
            instance.saberAColor = new HexColor(saberAColor);
            instance.saberBColor = new HexColor(saberBColor);
            instance.environmentColor0 = new HexColor(environmentColor0);
            instance.environmentColor1 = new HexColor(environmentColor1);
            instance.obstaclesColor = new HexColor(obstaclesColor);
            instance.environmentColor0Boost = new HexColor(environmentColor0Boost);
            instance.environmentColor1Boost = new HexColor(environmentColor1Boost);
            return instance;
        }
    }

}
