using DubsBadHygiene;
using System.Collections.Generic;
using Verse.AI;
using Verse;
using RimWorld;
using LetMeDrink.Extensions;

namespace LetMeDrink.JobDrivers
{
    public class JobDriver_AnimalDrinkFromBasin : JobDriver
    {
        public ContaminationLevel waterQuality;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref waterQuality, "waterQuality", ContaminationLevel.Treated);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 6, 0);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return new Toil().FailOnDestroyedNullOrForbidden(TargetIndex.A);
            yield return new Toil().FailOn(() => (pawn.CurJob.GetTarget(TargetIndex.A).Thing is Building_AssignableFixture building_AssignableFixture && !building_AssignableFixture.Working().Accepted) ? true : false);
            Toil chooseCell = ToilExtensions.FindReachableAdjacentCell(TargetIndex.A, TargetIndex.B);
            yield return chooseCell;
            yield return Toils_Reserve.Reserve(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
            Toil toil = ToilMaker.MakeToil("AnimalDrinkFromBasin");
            toil.defaultDuration = 500;
            toil.defaultCompleteMode = ToilCompleteMode.Delay;
            toil.FailOnDestroyedNullOrForbidden(TargetIndex.A);
            toil.AddEndCondition(delegate
            {
                Need_Thirst need_Thirst = pawn.needs.TryGetNeed<Need_Thirst>();
                if (need_Thirst == null)
                {
                    return JobCondition.Incompletable;
                }
                return (need_Thirst.CurLevel < 0.99f) ? JobCondition.Ongoing : JobCondition.Succeeded;
            });
            toil.tickAction = delegate
            {
                ContaminationLevel contam = ContaminationLevel.Treated;
                CompPipe compPipe = job.targetA.Thing.TryGetComp<CompPipe>();
                if (compPipe != null && !(job.targetA.Thing is Building_WaterTrough) && !compPipe.pipeNet.PullWater(0.04f, out contam))
                {
                    EndJobWith(JobCondition.Incompletable);
                }
                if (contam > waterQuality)
                {
                    waterQuality = contam;
                }
                pawn.needs?.TryGetNeed<Need_Thirst>()?.Drink();
            };
            toil.AddFinishAction(delegate
            {
                SanitationUtil.ContaminationCheckDrinking(pawn, waterQuality);
            });
            yield return toil;
            yield return Toils_Reserve.Release(TargetIndex.B);
            yield return Toils_Jump.Jump(chooseCell);
        }
    }
}
