

using System;
using System.Collections.Generic;

using HarmonyLib;

using TaleWorlds.CampaignSystem;

namespace AnotherAchievementModule
{
    [HarmonyPatch(typeof(Campaign))]
    static class CampaignPatch
    {
        private readonly static HashSet<string> _allowedModules;

        static CampaignPatch()
        {
            HashSet<string> strs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            strs.Add("Native");
            strs.Add("SandBoxCore");
            strs.Add("CustomBattle");
            strs.Add("SandBox");
            strs.Add("Multiplayer");
            strs.Add("BirthAndDeath");
            strs.Add("StoryMode");
            _allowedModules = strs;
        }

        [HarmonyPostfix]
        [HarmonyPatch("DeterminedSavedStats")]

        private static void DeterminedSavedStats(ref List<string> ____previouslyUsedModules, Campaign.GameLoadingType gameLoadingType)
        {
            if (Main.Settings!.ReEnableAchievementsInSaves)
            {
                ____previouslyUsedModules.RemoveAll((string x) => !_allowedModules.Contains(x)); 
            }
        }
    }
}
