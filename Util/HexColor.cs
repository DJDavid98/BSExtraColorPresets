using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace BSExtraColorPresets.Util
{
    public class HexColor
    {
        static Regex hexColorRegex = new Regex(@"^#?([\da-f]{3}|[\da-f]{6})$", RegexOptions.IgnoreCase);

        protected byte red = 0;
        protected byte green = 0;
        protected byte blue = 0;
        protected float alpha = 0;

        public HexColor()
        {
        }

        public HexColor(byte r, byte g, byte b, float alpha = 1)
        {
            this.red = r;
            this.green = g;
            this.blue = b;
            this.alpha = Converter.FloatToAlphaValue(alpha);
        }

        public HexColor(float r, float g, float b, float alpha = 1)
        {
            this.red = Converter.FloatToRgbValue(r);
            this.green = Converter.FloatToRgbValue(g);
            this.blue = Converter.FloatToRgbValue(b);
            this.alpha = Converter.FloatToAlphaValue(alpha);
        }

        public HexColor(string hexColorCode, float alpha = 1)
        {
            Match match = hexColorRegex.Match(hexColorCode);

            if (match.Success)
            {
                string hexDigits = match.Groups[1].Value;

                string redDigits, greenDigits, blueDigits;
                if (hexDigits.Length == 3)
                {
                    redDigits = $"{hexDigits[0]}{hexDigits[0]}";
                    greenDigits = $"{hexDigits[1]}{hexDigits[1]}";
                    blueDigits = $"{hexDigits[2]}{hexDigits[2]}";
                }
                else
                {
                    redDigits = hexDigits.Substring(0, 2);
                    greenDigits = hexDigits.Substring(2, 2);
                    blueDigits = hexDigits.Substring(4, 2);
                }

                red = Convert.ToByte(redDigits, 16);
                green = Convert.ToByte(greenDigits, 16);
                blue = Convert.ToByte(blueDigits, 16);
            }
            this.alpha = Converter.FloatToAlphaValue(alpha);
        }

        public HexColor(Color unityColor)
        {
            this.red = Converter.FloatToRgbValue(unityColor.r);
            this.green = Converter.FloatToRgbValue(unityColor.g);
            this.blue = Converter.FloatToRgbValue(unityColor.b);
            this.alpha = Converter.FloatToAlphaValue(unityColor.a);
        }

        public Color ToUnityColor()
        {
            return new Color(Converter.RgbToFloat(red), Converter.RgbToFloat(green), Converter.RgbToFloat(blue), alpha);
        }
    }
}
