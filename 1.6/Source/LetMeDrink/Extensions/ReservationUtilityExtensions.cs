using Verse;
using Verse.AI;

namespace LetMeDrink.Extensions
{
    public static class ReservationUtilityExtensions
    {
        public static bool CanReserveAnyAdjacentCells(this Pawn p, LocalTargetInfo target)
        {
            int num = 0;
            IntVec3 intVec;
            do
            {
                num++;
                if (num > 100)
                {
                    return false;
                }
                intVec = target.HasThing ? target.Thing.RandomAdjacentCell8Way() : target.Cell.RandomAdjacentCell8Way();
            }
            while (!intVec.Standable(p.Map) || !p.CanReserve(intVec) || !p.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly));
            return true;
        }
    }
}
