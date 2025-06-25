using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace AdRevenue
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class AdRevenue : BaseUnityPlugin
    {
        public static AdRevenue Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }

        public static int baseCredits;
        public static int multiCredits;
        public static float quotaMultiplier;
        public static bool displayMessage;
        public static string displayTitle;
        public static string displayDescription;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            baseCredits = Config.Bind<int>("General","BaseCredits",20,"Amount of credits given.").Value;
            multiCredits = Config.Bind<int>("General", "MultiCredits", 10, "Amount of credits given per each player after player 1.").Value;
            quotaMultiplier = Config.Bind<float>("General", "QuotaMultiplier", 1.5f, "Multiplier for the total credits given. Set to 1 to disable.").Value;
            displayMessage = Config.Bind<bool>("General", "DisplayMessage", true, "Display a message confirming that credits were given.").Value;
            displayTitle = Config.Bind<string>("General", "DisplayTitle", "Thank you!", "Title of the message box displayed after the ad.").Value;
            displayDescription = Config.Bind<string>("General", "DisplayDescription", "The company has paid you $bonus credits for your time.", "Description in the message box displayed after the ad. $bonus is replaced with the bonus given.").Value;

            Patch();


            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        }

        internal static void Patch()
        {
            Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

            Logger.LogDebug("Patching...");

            Harmony.PatchAll();

            Logger.LogDebug("Finished patching!");
        }

        internal static void Unpatch()
        {
            Logger.LogDebug("Unpatching...");

            Harmony?.UnpatchSelf();

            Logger.LogDebug("Finished unpatching!");
        }
    }
}
