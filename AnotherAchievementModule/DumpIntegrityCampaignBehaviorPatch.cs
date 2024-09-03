
using System;
using System.Linq;

using HarmonyLib;

using SandBox.CampaignBehaviors;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace AnotherAchievementModule
{
    [HarmonyPatch(typeof(DumpIntegrityCampaignBehavior))]
    static class DumpIntegrityCampaignBehaviorPatch
    {

        [HarmonyPostfix]
        [HarmonyPatch("CheckCheatUsage")]
        private static void CheckCheatUsage(ref bool __result)
        {
            if (Main.Settings!.Achievements)
            {
                __result = false;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("CheckIfModulesAreDefault")]
        private static bool CheckIfModulesAreDefault(ref bool __result)
        {
            bool flag = Campaign.Current.PreviouslyUsedModules.All<string>((string x) =>
            {
                if (x.Equals("Native", StringComparison.OrdinalIgnoreCase) || x.Equals("SandBoxCore", StringComparison.OrdinalIgnoreCase) || x.Equals("CustomBattle", StringComparison.OrdinalIgnoreCase) || x.Equals("SandBox", StringComparison.OrdinalIgnoreCase) || x.Equals("Multiplayer", StringComparison.OrdinalIgnoreCase) || x.Equals("BirthAndDeath", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                return x.Equals("StoryMode", StringComparison.OrdinalIgnoreCase);
            });
            if (!flag)
            {
                Debug.Print("Dump integrity is compromised due to non-default modules being used", 0, Debug.DebugColor.DarkRed, 17592186044416L);
                foreach (string previouslyUsedModule in Campaign.Current.PreviouslyUsedModules)
                {
                    Debug.Print(previouslyUsedModule, 0, Debug.DebugColor.DarkRed, 17592186044416L);
                }
            }
            __result = flag;
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch("IsGameIntegrityAchieved")]
        private static void IsGameIntegrityAchieved(ref TextObject reason, ref bool __result)
        {
            if (Main.Settings!.Achievements)
            {
                reason = null;
                __result = true;
            }
        }
    }
}
