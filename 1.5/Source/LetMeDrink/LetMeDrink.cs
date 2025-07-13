using System.Reflection;
using Verse;

namespace LetMeDrink
{
    public class LetMeDrink : Mod
    {
        public LetMeDrink(ModContentPack content) : base(content)
        {
            var harmony = new HarmonyLib.Harmony("keirloire.letmedrink");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
