namespace BSExtraColorPresets.UI
{
    public class MinimalExtraColorPreset : IMinimalExtraColorPreset
    {
        public string colorSchemeId { get; }
        public string name { get; }

        public static readonly MinimalExtraColorPreset randomItem = new MinimalExtraColorPreset("Random", "random");
        public static readonly MinimalExtraColorPreset randomUniqueItem = new MinimalExtraColorPreset("Shuffle", "randomUnique");

        public MinimalExtraColorPreset(ExtraColorPresetV2 preset)
        {
            name = preset.name;
            colorSchemeId = preset.colorSchemeId;
        }

        public MinimalExtraColorPreset(string name, string colorSchemeId)
        {
            this.name = name;
            this.colorSchemeId = colorSchemeId;
        }
    }
}