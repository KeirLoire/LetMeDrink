using HarmonyLib;
using LetMeDrink.Extensions;
using System.Reflection;
using Verse;
using Verse.AI;

namespace LetMeDrink.Patches
{
    [HarmonyPatch]
    public class ClosestSanitationPatch
    {
        public static MethodBase TargetMethod()
        {
            // ClosestSanitation is an internal class in DubsBadHygiene
            var type = AccessTools.TypeByName("DubsBadHygiene.ClosestSanitation");
            return AccessTools.Method(type, "UsableNow");
        }

        public static void Postfix(ref bool __result, Thing x, Pawn pawn, bool Urgent, float range)
        {
            if (x.def == LetMeDrinkDefOf.WaterTrough && pawn.RaceProps.Animal)
                __result = pawn.CanReserveAnyAdjacentCells(x);

            // Prevent non-animal pawns from using water troughs
            if (x.def == LetMeDrinkDefOf.WaterTrough && !pawn.RaceProps.Animal)
                __result = false;
        }
    }
}
