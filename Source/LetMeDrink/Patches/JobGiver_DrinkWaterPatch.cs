using DubsBadHygiene;
using HarmonyLib;
using Verse.AI;
using Verse;
using System;
using RimWorld;

namespace LetMeDrink.Patches
{
    public static class JobGiver_DrinkWaterPatch
    {
        static JobGiver_DrinkWaterPatch()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LetMeDrinkDefOf));
        }

        [HarmonyPatch(typeof(JobGiver_DrinkWater))]
        [HarmonyPatch(nameof(JobGiver_DrinkWater.TryGiveJob))]
        [HarmonyPatch(new Type[] { typeof(Pawn) })]
        public static class TryGiveJob
        {
            [HarmonyPostfix]
            public static void Postfix(ref Job __result, Pawn pawn)
            {
                if (__result == null || pawn == null)
                    return;

                if (__result.def == null)
                    return;

                if (__result.def == DubDef.DBHDrinkFromBasin 
                    && pawn.RaceProps.Animal
                    && __result.targetA.Thing.def != LetMeDrinkDefOf.PetWaterBowl)
                {
                    __result.def = LetMeDrinkDefOf.AnimalDrinkFromBasin;
                }
            }
        }
    }
}
