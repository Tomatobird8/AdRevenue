using HarmonyLib;
using System;
using Unity.Mathematics;

namespace AdRevenue.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    public class HUDManagerPatch
    {
        [HarmonyPatch("displayAd")]
        [HarmonyPostfix]
        private static void ApplyAdRevenue(HUDManager __instance)
        {
            int totalBonus = AdRevenue.baseCredits + AdRevenue.multiCredits * StartOfRound.Instance.connectedPlayersAmount;
            if (AdRevenue.quotaMultiplier != 1f)
            {
                totalBonus = (int)(math.max(AdRevenue.quotaMultiplier * TimeOfDay.Instance.timesFulfilledQuota, 1) * totalBonus);
            }
            if (StartOfRound.Instance.IsHost)
            {
                __instance.terminalScript.groupCredits += totalBonus;
                __instance.terminalScript.SyncGroupCreditsServerRpc(__instance.terminalScript.groupCredits, __instance.terminalScript.numberOfItemsInDropship);
            }
            if (AdRevenue.displayMessage)
            {
                string finalDescription = AdRevenue.displayDescription.Replace("$bonus", totalBonus.ToString());
                __instance.DisplayTip(AdRevenue.displayTitle, finalDescription);
            }
        }
    }
}
