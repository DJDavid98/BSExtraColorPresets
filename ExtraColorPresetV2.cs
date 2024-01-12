using BeatSaberMarkupLanguage.Attributes;
using BSExtraColorPresets.UI;
using BSExtraColorPresets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BSExtraColorPresets
{
    public class ExtraColorPresetV2 : IMinimalExtraColorPreset
    {
        public static HexColor DEFAULT_SABER_A_COLOR = new HexColor(168, 32, 32);
        public static HexColor DEFAULT_SABER_B_COLOR = new HexColor(32, 100, 168);
        public static HexColor DEFAULT_ENV_0_COLOR = new HexColor(192, 48, 48);
        public static HexColor DEFAULT_ENV_1_COLOR = new HexColor(48, 152, 255);
        public static HexColor DEFAULT_OBSTACLES_COLOR = new HexColor(255, 48, 48);
        public static HexColor DEFAULT_ENV_0_BOOST_COLOR = new HexColor();
        public static HexColor DEFAULT_ENV_1_BOOST_COLOR = new HexColor();

        public virtual string colorSchemeId { get; set; } = GenerateUniqueId();

        [UIValue("preset-name")]
        public virtual string name { get; set; } = GenerateName();

        [UIValue("saber-a-color")]
        public virtual HexColor saberAColor { get; set; } = DEFAULT_SABER_A_COLOR;

        [UIValue("saber-b-color")]
        public virtual HexColor saberBColor { get; set; } = DEFAULT_SABER_B_COLOR;

        [UIValue("env-0-color")]
        public virtual HexColor environmentColor0 { get; set; } = DEFAULT_ENV_0_COLOR;

        [UIValue("env-1-color")]
        public virtual HexColor environmentColor1 { get; set; } = DEFAULT_ENV_1_COLOR;

        [UIValue("obstacles-color")]
        public virtual HexColor obstaclesColor { get; set; } = DEFAULT_OBSTACLES_COLOR;

        public virtual HexColor environmentColor0Boost { get; set; } = DEFAULT_ENV_0_BOOST_COLOR;
        public virtual HexColor environmentColor1Boost { get; set; } = DEFAULT_ENV_1_BOOST_COLOR;

        public static string GenerateUniqueId()
        {
            return $"ExtraColorScheme{Guid.NewGuid()}";
        }
        public static string GenerateName(int index = 0)
        {
            return $"Preset {index}";
        }

        public ColorScheme ToColorScheme(ColorScheme fallbackScheme)
        {
            return new ColorScheme(
                colorSchemeId,
                "",
                true,
                name,
                false,
                saberAColor.ToUnityColor(),
                saberBColor.ToUnityColor(),
                environmentColor0.ToUnityColor(),
                environmentColor1.ToUnityColor(),
                fallbackScheme.environmentColorW,
                true,
                environmentColor0Boost.ToUnityColor(),
                environmentColor1Boost.ToUnityColor(),
                fallbackScheme.environmentColorWBoost,
                obstaclesColor.ToUnityColor()
            );
        }

        [UIAction("edit")]
        public void EditAction()
        {
            PresetManagerSettings.Instance.EditPreset(this);
        }

        [UIAction("delete")]
        public void DeleteAction()
        {
            PresetManagerSettings.Instance.ConfirmDeletePreset(this);
        }
    }

}
