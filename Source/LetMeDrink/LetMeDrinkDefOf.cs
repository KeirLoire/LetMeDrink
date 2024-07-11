using RimWorld;
using Verse;

namespace LetMeDrink
{
    [DefOf]
    public static class LetMeDrinkDefOf
    {
        public static JobDef AnimalDrinkFromBasin;
        public static ThingDef PetWaterBowl;
        public static ThingDef WaterTrough;

        static LetMeDrinkDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LetMeDrinkDefOf));
        }
    }
}
