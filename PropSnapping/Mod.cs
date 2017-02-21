using ICities;
using PropSnapping.OptionsFramework.Extensions;

namespace PropSnapping
{
    public class Mod : IUserMod
    {
        public string Name => "Prop Snapping";
        public string Description => "Allows to snap props to buildings, roads - just like in asset editor!";

        public void OnSettingsUI(UIHelperBase helper)
        {
            helper.AddOptionsGroup<Options>();
        }
    }
}