using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using ModSettings;
using UnityEngine;

namespace More_Autosaves
{
    public class Implementation : MelonMod
    {
        public override void OnApplicationLateStart()
        {
            MelonDebug.Msg($"[{Info.Name}] Version {Info.Version} loaded!");
            lastsavetime = 0;

        }

        public override void OnApplicationStart()
        {
            Settings.OnLoad();
        }
        static float saveperiod = 0;
        public static void UpdateSavePeriod()
        {
                if (Settings.options.EnableMod && Settings.options.TimeSave) saveperiod = Settings.options.Minutes * 60 + Settings.options.Seconds;
                if (saveperiod < 5) saveperiod = 5;
        }

        static float delay = 3;
        static bool waitingforsave = false;
        public static void DelaySeconds(float seconds)
        {
            delay = seconds;
            waitingforsave = true;
        }

        void UpdateforDelaySeconds()
        {
            if (waitingforsave)
            {
                delay -= Time.deltaTime;
                if (delay <= 0)
                {
                    waitingforsave = false;
                    Save();
                }
            }

        }

        public static bool ShouldSaveGameonQuit()
        {
            if (Settings.options.EnableMod && Settings.options.ExitSave)
            {
                if (Settings.options.xinExitSave && GameManager.GetWeatherComponent() != null && GameManager.GetWeatherComponent().IsIndoorEnvironment()) return false;
                return true;
            }
            return false;
        }

        public static float lastsavetime = 0;
        public static void Save(bool allowed = true)
        {
            if (allowed)
            {
                GameManager.TriggerSurvivalSaveAndDisplayHUDMessage();
            }
        }
        void AttemptPeriodicAutosave()
        {
            if(Settings.options.EnableMod && Settings.options.TimeSave)
            {
                if (Settings.options.xinTimeSave && GameManager.GetWeatherComponent() != null && GameManager.GetWeatherComponent().IsIndoorEnvironment()) return;
                if (lastsavetime > saveperiod) Save();
            }
        }

        void SaveifTriggerAllowed()
        {
            bool TriggerAllowed()
            {
                return ShouldSaveGameonExitBuilding() || ShouldSaveGameonBlizzard();
            }
            if (TriggerAllowed()) Save();
        }

        bool blizzstart = false;
        bool ShouldSaveGameonBlizzard()
        {
            if(Settings.options.EnableMod && Settings.options.BlizzSave && blizzstart)
            {
                blizzstart = false;
                return true;
            }
            return false;
        }

        bool lastblizzard = false;
        void CheckforBlizzards()
        {
            if(Settings.options.EnableMod && Settings.options.BlizzSave)
            {
                bool blizzard = GameManager.GetWeatherComponent() != null && GameManager.GetWeatherComponent().IsBlizzard();
                if (blizzard && !lastblizzard) blizzstart = true;
                lastblizzard = blizzard;
            }
        }

        bool ShouldSaveGameonExitBuilding()
        {
            if(Settings.options.EnableMod && Settings.options.OutSave)
            {
                if (GameManager.m_SceneTransitionData == null || GameManager.GetWeatherComponent() == null) return false;
                return !GameManager.m_SceneTransitionData.m_TeleportPlayerSaveGamePosition && !GameManager.GetWeatherComponent().IsIndoorEnvironment() && GameManager.m_SceneTransitionData.m_SpawnPointName != null;
            }
            return false;
        }

        static bool saveframe = false;
        public static void SaveNextFrame()
        {
            saveframe = true;
        }

        void CheckForSaveFrame()
        {
            if (saveframe)
            {
                saveframe = false;
                Save();
            }
            
        }

        public static void SaveifAnimalnotFleeBanned(BaseAi animal)
        {
            bool AnimalIsBanned() 
            {
                if(Settings.options.FleeBanAnimals)
                {
                    if (Settings.options.FleeBanRabbits && animal.m_AiSubType == AiSubType.Rabbit) return true;
                    if (Settings.options.FleeBanStags && animal.m_AiSubType == AiSubType.Stag) return true;
                    if (Settings.options.FleeBanWolves && animal.m_AiSubType == AiSubType.Wolf) return true;
                    if (Settings.options.FleeBanBears && animal.m_AiSubType == AiSubType.Bear) return true;
                }
                return false;
            }
            if(!AnimalIsBanned()) Save();
        }

        public static void SaveifAnimalnotAttackBanned(BaseAi animal)
        {
            bool AnimalIsBanned()
            {
                if (Settings.options.AttackBanAnimals)
                {
                    if (Settings.options.AttackBanWolves && animal.m_AiSubType == AiSubType.Wolf) return true;
                    if (Settings.options.AttackBanBears && animal.m_AiSubType == AiSubType.Bear) return true;
                    if (Settings.options.AttackBanMoose && animal.m_AiSubType == AiSubType.Moose) return true;
                }
                return false;
            }
            if (!AnimalIsBanned()) Save();
        }

        void HideSaveIcon()
        {
            if(Settings.options.EnableMod && Settings.options.HideIcon && InterfaceManager.m_Panel_SaveIcon != null) InterfaceManager.m_Panel_SaveIcon.Enable(false);
        }

        public static bool Aware(BaseAi ai)
        {
            switch (ai.GetAiMode())
            {
                case AiMode.Attack:
                case AiMode.PassingAttack:
                case AiMode.Flee:
                case AiMode.Stalking:
                case AiMode.HoldGround:
                    return true;
            }
            return false;
        }

        void CheckforQuickSave()
        {
            if (Settings.options.EnableMod && Settings.options.SurvivalQuickSave && InputManager.instance != null && !GameManager.IsStoryMode()) 
            {
                if(InputManager.GetQuickSavePressed(InputManager.m_CurrentContext)) Save();
                if (InputManager.GetQuickLoadPressed(InputManager.m_CurrentContext)) GameManager.LoadActiveSaveGame();
            } 
        }

        public static bool CanExitSave = false;
        void CheckforExitSave() 
        { if (Settings.options.EnableMod && Settings.options.ExitSave && CanExitSave && lastsavetime==0) 
            {
                CanExitSave = false;
                GameManager.GetLogComponent().WriteLogToFile();
                GameManager.LoadMainMenu();
            } 
        }

        public override void OnUpdate()
        {
            lastsavetime += Time.deltaTime;
            CheckforExitSave();
            CheckforQuickSave();
            UpdateforDelaySeconds();
            CheckForSaveFrame();
            HideSaveIcon();
            AttemptPeriodicAutosave();
            CheckforBlizzards();
            SaveifTriggerAllowed();
        }

    }

}






