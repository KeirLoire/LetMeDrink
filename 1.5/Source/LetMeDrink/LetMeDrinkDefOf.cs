using RimWorld;
using Verse;

namespace LetMeDrink
{
    [DefOf]
    public static class LetMeDrinkDefOf
    {
        [MayRequire("bhdlitemodestub")]
        public static JobDef AnimalDrinkFromBasin;
        [MayRequire("bhdlitemodestub")]
        public static ThingDef PetWaterBowl;
        [MayRequire("bhdlitemodestub")]
        public static ThingDef WaterTrough;

        static LetMeDrinkDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LetMeDrinkDefOf));
        }
    }
}
