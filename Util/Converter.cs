using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSExtraColorPresets.Util
{
    internal class Converter
    {
        internal static float RgbToFloat(float value)
        {
            return value / 255f;
        }

        internal static byte FloatToRgbValue(float rgbValue)
        {
            return (byte)Math.Max(0, Math.Min(255, Math.Round(rgbValue * 255)));
        }

        internal static float FloatToAlphaValue(float alphaValue)
        {
            return (float)Math.Max(0, Math.Min(1, alphaValue));
        }
    }
}
