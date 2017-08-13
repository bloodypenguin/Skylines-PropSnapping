using PropSnapping.OptionsFramework.Attibutes;

namespace PropSnapping
{
    [Options("PropSnapping")]
    public class Options
    {
        [Checkbox("Don't update prop Y coordinate on terrain modification")]
        public bool dontUpdateYOnTerrainModification { set; get; } = true;

        [Checkbox("Allow prop to submerge")]
        public bool allowToSubmerge { set; get; } = false;
    }
}