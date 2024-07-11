using Verse.AI;
using Verse;

namespace LetMeDrink.Extensions
{
    public static class ToilExtensions
    {
        // Find reachable adjacent cells to the target cell, make job incompletable if it fails
        public static Toil FindReachableAdjacentCell(TargetIndex adjacentToInd, TargetIndex cellInd)
        {
            Toil findCell = ToilMaker.MakeToil("FindReachableAdjacentCell");
            findCell.initAction = delegate
            {
                Pawn actor = findCell.actor;
                Job curJob = actor.CurJob;
                LocalTargetInfo target = curJob.GetTarget(adjacentToInd);
                if (target.HasThing && (!target.Thing.Spawned || target.Thing.Map != actor.Map))
                {
                    actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
                    return;
                }
                else
                {
                    int num = 0;
                    IntVec3 intVec;
                    do
                    {
                        num++;
                        if (num > 100)
                        {
                            actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
                            return;
                        }
                        intVec = ((!target.HasThing) ? target.Cell.RandomAdjacentCell8Way() : target.Thing.RandomAdjacentCell8Way());
                    }
                    while (!intVec.Standable(actor.Map) || !actor.CanReserve(intVec) || !actor.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly));
                    curJob.SetTarget(cellInd, intVec);
                }
            };
            return findCell;
        }
    }
}
