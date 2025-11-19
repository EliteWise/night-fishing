using System;
using System.IO;
using HarmonyLib;
using Microsoft.VisualBasic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Tools;
using StardewValley.Menus;
using StardewValley.Minigames;

namespace NightFishing
{
    public class ModEntry : Mod
    {
        private ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Tools.FishingRod), nameof(StardewValley.Tools.FishingRod.pullFishFromWater)),
                prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.PullFishFromWater_Prefix))
            );

            this.Config = this.Helper.ReadConfig<ModConfig>();
            bool exampleBool = this.Config.ExampleBoolean;

            helper.Events.GameLoop.TimeChanged += this.OnTimeChanged;
        }

        static bool isNightTime;

        public static void PullFishFromWater_Prefix(string fishId,
            int fishSize,
            int fishQuality,
            int fishDifficulty,
            bool treasureCaught,
            bool wasPerfect,
            bool fromFishPond,
            string setFlagOnCatch,
            bool isBossFish,
            ref int numCaught)
        {
            if (!isNightTime) return;

            numCaught *= 2;
        }

        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            if (e.NewTime == 2400)
            {
                isNightTime = true;
                this.Monitor.Log($"{e.NewTime}.", LogLevel.Debug);
            } else if (e.NewTime == 600)
            {
                isNightTime = false;
            }
        }
    }
}

