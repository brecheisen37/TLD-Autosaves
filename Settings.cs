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
                field.Name == nameof(SurvivalQuickSave) ||
                field.Name == nameof(OutSave) ||
                field.Name == nameof(BlizzSave) ||
                field.Name == nameof(ExitSave) ||
                field.Name == nameof(xinExitSave) ||
                field.Name == nameof(TimeSave) ||
                field.Name == nameof(xinTimeSave) ||
                field.Name == nameof(Minutes) ||
                field.Name == nameof(Seconds) ||
                field.Name == nameof(WoundSave) ||
                field.Name == nameof(StalkSave) ||
                field.Name == nameof(LoadAggro) ||
                field.Name == nameof(AttackSave) ||
                field.Name == nameof(AttackBanAnimals) ||
                field.Name == nameof(AttackBanWolves) ||
                field.Name == nameof(AttackBanBears) ||
                field.Name == nameof(AttackBanMoose) ||
                field.Name == nameof(FleeSave) ||
                field.Name == nameof(Passive) ||
                field.Name == nameof(FleeBanAnimals) ||
                field.Name == nameof(FleeBanRabbits) ||
                field.Name == nameof(FleeBanStags) ||
                field.Name == nameof(FleeBanWolves) ||
                field.Name == nameof(FleeBanBears))
            {
                RefreshFields();
            }
        }
        public void RefreshFields()
        {
            SetFieldVisible(nameof(SurvivalQuickSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(OutSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(BlizzSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(ExitSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(xinExitSave), Settings.options.EnableMod && Settings.options.ExitSave);
            SetFieldVisible(nameof(TimeSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(xinTimeSave), Settings.options.EnableMod && Settings.options.TimeSave);
            SetFieldVisible(nameof(Minutes), Settings.options.EnableMod && Settings.options.TimeSave);
            SetFieldVisible(nameof(Seconds), Settings.options.EnableMod && Settings.options.TimeSave);
            SetFieldVisible(nameof(WoundSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(StalkSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(LoadAggro), Settings.options.EnableMod);
            SetFieldVisible(nameof(AttackSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(AttackBanAnimals), Settings.options.EnableMod && Settings.options.AttackSave);
            SetFieldVisible(nameof(AttackBanWolves), Settings.options.EnableMod && Settings.options.AttackSave && Settings.options.AttackBanAnimals);
            SetFieldVisible(nameof(AttackBanBears), Settings.options.EnableMod && Settings.options.AttackSave && Settings.options.AttackBanAnimals);
            SetFieldVisible(nameof(AttackBanMoose), Settings.options.EnableMod && Settings.options.AttackSave && Settings.options.AttackBanAnimals);
            SetFieldVisible(nameof(FleeSave), Settings.options.EnableMod);
            SetFieldVisible(nameof(Passive), Settings.options.EnableMod && Settings.options.FleeSave);
            SetFieldVisible(nameof(FleeBanAnimals), Settings.options.EnableMod && Settings.options.FleeSave);
            SetFieldVisible(nameof(FleeBanRabbits), Settings.options.EnableMod && Settings.options.FleeSave && Settings.options.FleeBanAnimals);
            SetFieldVisible(nameof(FleeBanStags), Settings.options.EnableMod && Settings.options.FleeSave && Settings.options.FleeBanAnimals);
            SetFieldVisible(nameof(FleeBanWolves), Settings.options.EnableMod && Settings.options.FleeSave && Settings.options.FleeBanAnimals);
            SetFieldVisible(nameof(FleeBanBears), Settings.options.EnableMod && Settings.options.FleeSave && Settings.options.FleeBanAnimals);
        }



        [Section("General")]

        [Name("Enable Mod")]
        [Description("Turn mod off without uninstalling.")]
        public bool EnableMod = true;

        [Name("Survival Quicksaves")]
        [Description("Allow Quicksaving in Survival Mode")]
        public bool SurvivalQuickSave = false;

        [Name("Hide the Save Icon")]
        [Description("Hides the icon in the bottom right that says \"Saving...\"")]
        public bool HideIcon = false;

        [Name("Save on Exit Building")]
        [Description("Will save on scene transition to an outdoor location.")]
        public bool OutSave = true;

        [Name("Save on Blizzard Start")]
        [Description("Will save when a blizzard begins.")]
        public bool BlizzSave = true;

        [Name("Save on Quit")]
        [Description("Will save when you click \"Quit\" then \"Confirm\" from the pause menu.")]
        public bool ExitSave = true;

        [Name("Only Outside")]
        [Description("Will only save when you quit outside.")]
        public bool xinExitSave = false;

        [Section("Periodic Saves")]

        [Name("Save Periodically")]
        [Description("Will save every x seconds if enabled.")]
        public bool TimeSave = true;

        [Name("Only Outside")]
        [Description("Will suppress periodic saves indoors.")]
        public bool xinTimeSave = false;

        [Name("Autosave Time Period")]
        [Description("Save every x seconds.")]
        [Slider(5,64,60)]
        public float Seconds = 30f;

        [Name("Additional Time")]
        [Description("Add x minutes to timer.")]
        [Slider(0, 59, 60)]
        public float Minutes = 0f;

        [Section("Animal Saves")]

        [Name("Save on First Hit")]
        [Description("Will Save When you first wound an animal.")]
        public bool WoundSave = false;

        [Name("Save on Stalk")]
        [Description("Will Save When a predator starts to stalk you.")]
        public bool StalkSave = false;

        [Name("Aggro Animals on Load")]
        [Description("Makes nearby animals aggressive when you load a save(recommended).")]
        public bool LoadAggro = true;

        [Name("Save on Attack")]
        [Description("Will save when attacked.(filters available)")]
        public bool AttackSave = false;

        [Name("Enable Attack Blacklist")]
        [Description("Save on attack will only apply to animals you haven't chosen to exclude.")]
        public bool AttackBanAnimals = true;

        [Name("Exclude Wolves")]
        [Description("Wolves attacking will not trigger a save.(recommended)")]
        public bool AttackBanWolves = true;

        [Name("Exclude Bears")]
        [Description("Bears attacking will not trigger a save.")]
        public bool AttackBanBears = false;

        [Name("Exclude Moose")]
        [Description("Moose attacking will not trigger a save.")]
        public bool AttackBanMoose = false;

        [Name("Save on Flee")]
        [Description("Will Save When you scare off an animal.(filters available)")]
        public bool FleeSave = true;

        [Name("Only Passive")]
        [Description("Won't save if you scare off an attacking predator.")]
        public bool Passive = true;

        [Name("Enable Flee Blacklist")]
        [Description("Save on flee will only apply to animals you haven't chosen to exclude.")]
        public bool FleeBanAnimals = true;

        [Name("Exclude Rabbits")]
        [Description("Rabbits fleeing will not trigger a save.")]
        public bool FleeBanRabbits = true;

        [Name("Exclude Deer/Stags")]
        [Description("Stags fleeing will not trigger a save.")]
        public bool FleeBanStags = true;

        [Name("Exclude Wolves")]
        [Description("Wolves fleeing will not trigger a save.")]
        public bool FleeBanWolves = false;

        [Name("Exclude Bears")]
        [Description("Bears fleeing will not trigger a save.")]
        public bool FleeBanBears = false;

    }




    internal static class Settings
    {
        public static SettingsMain options;

        public static void OnLoad()
        {
            options = new SettingsMain();
            options.AddToModSettings("AutoSave");
            options.RefreshFields();
            Implementation.UpdateSavePeriod();
        }

    }


}
