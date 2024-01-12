
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace BSExtraColorPresets.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        [NonNullable]
        public virtual bool Enabled { get; set; } = false;

        public virtual string SelectedPresetId { get; set; } = null;

#pragma warning disable CS0618 // Type or member is obsolete
        [UseConverter(typeof(ListConverter<ExtraColorPreset>))]
        [Obsolete("Use ExtraColorPresetsV2 instead")]
        public virtual List<ExtraColorPreset>? ExtraColorPresets { get; set; }
#pragma warning restore CS0618 // Type or member is obsolete

        [NonNullable]
        [UseConverter(typeof(ListConverter<ExtraColorPresetV2>))]
        public virtual List<ExtraColorPresetV2> ExtraColorPresetsV2 { get; set; } = new List<ExtraColorPresetV2>();

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {
            // Do stuff after config is read from disk.
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
        }
    }
}
