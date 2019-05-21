//this class handles all the setup for every other 
//class in the mod
//
//this is like a huge start method
//












using ModAPI.Attributes;
using TheForest.Utils;

namespace BuilderMenu
{
    public class ClockMod : Clock
    {
        protected override void Awake()
        {
            EditorInitializer.Initialize();
            base.Awake();
        }
        
    }
    internal class SaveOverride : PlayerStats
    {
        [Priority(20)]
        public override void JustSave()
        {
            base.JustSave();
            EditorMethods.SaveBlueprints();
        }
    }
}
