using PropSnapping.OptionsFramework.Attibutes;

namespace PropSnapping
{
    [Options("PropSnapping")]
    public class Options
    {
        public Options()
        {
            dontUpdateYOnTerrainModification = true;
        }


        [Checkbox("Don't update tree Y coordinate on terrain modification")]
        public bool dontUpdateYOnTerrainModification { set; get; }

    }
}