using System.IO;
using System.Reflection;
using ModSettings;
using UnityEngine;

namespace More_Autosaves
{
    internal class SettingsMain : JsonModSettings
    {
        protected override void OnConfirm()
        {
            base.OnConfirm();
            Implementation.UpdateSavePeriod();
        }

        protected override void OnChange(FieldInfo field, object oldValue, object newValue)
        {
            if (field.Name == nameof(EnableMod) ||
                field.Name == nameof(ExitSave) ||
                field.Name == nameof(BlizzSave) ||
                field.Name == nameof(TimeSave) ||
                field.Name == nameof(Minutes) ||
                field.Name == nameof(Seconds) ||
                field.Name == nameof(WoundSave) ||
                field.Name == nameof(StalkSave))
            {
                RefreshFields();
            }
        }
        public void RefreshFields()
        {
            SetFieldVisible(nameof(ExitSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(BlizzSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(TimeSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(Minutes), Settings.options.EnableMod && Settings.options.TimeSave);
            SetFieldVisible(nameof(Seconds), Settings.options.EnableMod && Settings.options.TimeSave);
            SetFieldVisible(nameof(WoundSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(StalkSave), Settings.options.EnableMod);
        }



        [Section("General")]

        [Name("Enable Mod")]
        [Description("Turn mod off without uninstalling.")]
        public bool EnableMod = true;

        [Name("Hide the Save Icon")]
        [Description("Hides the icon in the bottom right that says \"Saving...\"")]
        public bool HideIcon = true;

        [Name("Save on Exit Building")]
        [Description("Will save on scene transition to an outdoor location.")]
        public bool ExitSave = true;

        [Name("Save on Blizzard Start")]
        [Description("Will save when a blizzard begins.")]
        public bool BlizzSave = true;

        [Section("Periodic Saves")]

        [Name("Save Periodically")]
        [Description("Will save every x seconds if enabled.")]
        public bool TimeSave = true;

        [Name("Autosave Time Period")]
        [Description("Save every x seconds.")]
        [Slider(5,59,55)]
        public float Seconds = 30f;

        [Name("Additional Time")]
        [Description("Add x minutes to timer.")]
        [Slider(0, 59, 60)]
        public float Minutes = 0f;

        [Section("Combat Saves")]

        [Name("Save on Animal Hurt")]
        [Description("Will Save When you wound an animal.")]
        public bool WoundSave = false;

        [Name("Save on Wolf Aggro")]
        [Description("Will Save When a wolf starts to follow you.")]
        public bool StalkSave = false;

    }




    internal static class Settings
    {
        public static SettingsMain options;

        public static void OnLoad()
        {
            options = new SettingsMain();
            options.AddToModSettings("More Autosaves");
            Settings.options.RefreshFields();
        }

    }


}
