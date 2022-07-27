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
    public class PatchClass
    {


    }

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
            HarmonyInstance.PatchAll();
        }


        public override void OnSceneWasInitialized(int level, string name) // finished loading scene
        {

            
        }

        static float saveperiod = 0;
        public static void UpdateSavePeriod()
        {
                if (Settings.options.EnableMod && Settings.options.TimeSave) saveperiod = Settings.options.Minutes * 60 + Settings.options.Seconds;
                if (saveperiod < 5) saveperiod = 5;
        }
        void AttemptPeriodicAutosave()
        {
            if(Settings.options.EnableMod && Settings.options.TimeSave)
            {
                if (saveperiod == 0) UpdateSavePeriod();
                lastsavetime += Time.deltaTime;
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
            if(Settings.options.EnableMod && Settings.options.ExitSave)
            {
                if (GameManager.m_SceneTransitionData == null || GameManager.GetWeatherComponent() == null) return false;
                return !GameManager.m_SceneTransitionData.m_TeleportPlayerSaveGamePosition && !GameManager.GetWeatherComponent().IsIndoorEnvironment() && GameManager.m_SceneTransitionData.m_SpawnPointName != null;
            }
            return false;
        }

        static float lastsavetime = 0;
        public static void Save()
        {
            GameManager.TriggerSurvivalSaveAndDisplayHUDMessage();
            lastsavetime = 0;
        }

        void HideSaveIcon()
        {
            if(Settings.options.EnableMod && Settings.options.HideIcon && InterfaceManager.m_Panel_SaveIcon != null) InterfaceManager.m_Panel_SaveIcon.Enable(false);
        }

        BaseAi closestwolf = new BaseAi();
        void Initializeclosestwolf()
        {
            if (BaseAiManager.s_ClosestAiToPlayer != null)
            {
                closestwolf = BaseAiManager.s_ClosestAiToPlayer;
            }
        }

        
        bool lastaggressive = false;
        void CheckforWolfStalk()
        {
            Initializeclosestwolf();
            if (Settings.options.EnableMod && Settings.options.StalkSave && closestwolf != null)
            {
                bool aggressive = closestwolf.CurrentAiModeAllowsHoldGround();
                if (aggressive && !lastaggressive) Save();
                lastaggressive = aggressive;
            }
        }



        bool OnLoadHasBeenCalled = false;
        void CheckforOnStart()
        {
            void OnStart()
            {
                if(Settings.options.EnableMod && Settings.options.StalkSave)
                {
                    Initializeclosestwolf();
                    if(closestwolf != null)
                    {
                        closestwolf.MaybeForceStalkPlayer();
                        //closestwolf.ForceSetPlayerTarget();
                        //closestwolf.SetAiMode(AiMode.Stalking);
                    }

                }
            }

            if (!OnLoadHasBeenCalled && GameManager.GetPlayerManagerComponent() != null)
            {
                OnStart();
                OnLoadHasBeenCalled = true;
            }
        }

        public override void OnUpdate()
        {
            HideSaveIcon();
            //CheckforBleed();
            CheckforOnStart();
            AttemptPeriodicAutosave();
            CheckforBlizzards();
            CheckforWolfStalk();
            SaveifTriggerAllowed();
        }

    }

}






