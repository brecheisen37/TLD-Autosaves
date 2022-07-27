using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MelonLoader;

namespace More_Autosaves
{
	[HarmonyPatch(typeof(AiTarget), "ApplyDamage")]
	public class SaveonWound
	{
		public static bool Prefix(AiTarget __instance)
		{
			MelonLogger.Msg("owie");
			if (Settings.options.EnableMod && Settings.options.WoundSave && !__instance.m_BaseAi.m_Wounded) Implementation.Save();

			// do not skip original method          
			return true;
		}
	}
}
