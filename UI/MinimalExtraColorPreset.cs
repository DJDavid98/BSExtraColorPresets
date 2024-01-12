namespace BSExtraColorPresets.UI
{
    public class MinimalExtraColorPreset
    {
        public string colorSchemeId;
        public string name;

        public static readonly MinimalExtraColorPreset randomItem = new MinimalExtraColorPreset("Random", "random");

        public MinimalExtraColorPreset(ExtraColorPresetV2 preset)
        {
            this.name = preset.name;
            this.colorSchemeId = preset.colorSchemeId;
        }

        public MinimalExtraColorPreset(string name, string colorSchemeId)
        {
            this.name = name;
            this.colorSchemeId = colorSchemeId;
        }
    }
}