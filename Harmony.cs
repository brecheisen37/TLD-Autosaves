using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace More_Autosaves
{
	class Patches
	{
		[HarmonyPatch(typeof(BaseAi), "ApplyDamage", new Type[] { typeof(float), typeof(float), typeof(DamageSource), typeof(string) })]
		public static class SaveonWound
		{
			public static bool Prefix(BaseAi __instance)
			{
				MelonLogger.Msg("owie");
				if (Settings.options.EnableMod && Settings.options.WoundSave && !Implementation.Aware(__instance)) Implementation.SaveNextFrame();
				// do not skip original method          
				return true;
			}
		}

		[HarmonyPatch(typeof(BaseAi), "EnterStalking")]
		public static class SaveonStalk
		{
			public static void Postfix(BaseAi __instance)
			{
				if (Settings.options.EnableMod && Settings.options.StalkSave)
					Implementation.DelaySeconds(5f);
			}
		}

		[HarmonyPatch(typeof(BaseAi), "EnterAttackModeIfPossible")]
		public static class SaveonAttack
		{
			public static void Postfix(BaseAi __instance)
			{
				if (Settings.options.EnableMod && Settings.options.AttackSave)
					Implementation.SaveifAnimalnotAttackBanned(__instance);
			}
		}

		[HarmonyPatch(typeof(BaseAi), "Start")]
		public static class StartStalk
		{
			public static void Postfix(BaseAi __instance)
			{
				if (Settings.options.EnableMod && Settings.options.LoadAggro && GameManager.GetPlayerManagerComponent() != null && GameManager.GetPlayerManagerComponent().IsFirstFrame())
                {
					if(__instance.CanPlayerBeReached(GameManager.GetPlayerManagerComponent().transform.position))
					{
						__instance.m_ChanceAttackFightOrFlight = 100f;
						if(__instance.m_AiMoose != null) __instance.m_AiMoose.m_HoldBeforeAttackDelaySeconds = 0;
						__instance.m_PlayedAttackStartAnimation = true;
						__instance.MaybeForceStalkPlayer();
						__instance.MaybeForceFasterStalkingSpeed();
						__instance.MaybeEnterAttackModeWhenStalking();
						__instance.SetAiMode(AiMode.Attack);
						__instance.ProcessAttack();

					}						
						
				}
					
			}
		}

		[HarmonyPatch(typeof(BaseAi), "EnterFlee")]
		public static class SaveonFlee
		{
			public static bool Prefix(BaseAi __instance)
			{
				if (Settings.options.EnableMod && Settings.options.FleeSave)
					if (!Implementation.Aware(__instance) || !Settings.options.Passive)
						Implementation.SaveifAnimalnotFleeBanned(__instance);
             
				return true;
			}
		}

		[HarmonyPatch(typeof(SaveGameSystem), "SaveGame")]
		public static class LastSaveTime
		{
			public static void Postfix()
			{
				if (Settings.options.EnableMod) Implementation.lastsavetime = 0;
			}
		}

		[HarmonyPatch(typeof(Panel_PauseMenu), "DoQuitGame")]
		public static class SaveOnQuit
		{
			public static bool Prefix(Panel_PauseMenu __instance)
			{
				if (Implementation.ShouldSaveGameonQuit())
				{
					GameManager.OnGameQuit();
					__instance.OnDone();
					Implementation.Save();
					Implementation.CanExitSave = true;
					return false;
				}
				return true;
			}
		}
	}
}