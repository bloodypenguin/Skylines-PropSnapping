using ColossalFramework;
using PropSnapping.OptionsFramework;
using PropSnapping.Redirection;
using UnityEngine;

namespace PropSnapping.Detour
{
    [TargetType(typeof(PropInstance))]
    public struct PropInstanceDetour
    {
        [RedirectMethod]
        public static void CalculateProp(ref PropInstance prop, ushort propID)
        {
            //begin mod
            //end mod
        }

        [RedirectMethod]
        public static void AfterTerrainUpdated(ref PropInstance prop, ushort propID, float minX, float minZ, float maxX, float maxZ)
        {
            if (((int)prop.m_flags & 3) != 1 || ((int)prop.m_flags & 32) != 0)
                return;
            Vector3 position = prop.Position;
            position.y = Singleton<TerrainManager>.instance.SampleDetailHeight(position);
            ushort num = (ushort)Mathf.Clamp(Mathf.RoundToInt(position.y * 64f), 0, (int)ushort.MaxValue);
            if ((int)num == (int)prop.m_posY)
                return;
            bool blocked1 = prop.Blocked;
            //begin mod
            if (ToolsModifierControl.GetCurrentTool<TerrainTool>() == null || OptionsWrapper<Options>.Options.dontUpdateYOnTerrainModification)
            {
                prop.m_posY = prop.m_posY > num ? prop.m_posY : num;
            }
            else
            {
                prop.m_posY = num;
            }
            //end mod
            bool blocked2 = prop.Blocked;
            if (blocked2 != blocked1)
            {
                Singleton<PropManager>.instance.UpdateProp(propID);
            }
            else
            {
                if (blocked2)
                    return;
                Singleton<PropManager>.instance.UpdatePropRenderer(propID, true);
            }
        }

        [RedirectReverse]
        private static void CheckOverlap(ref PropInstance prop, ushort propID)
        {
            UnityEngine.Debug.LogError("Failed to redirect CheckOverlap()");
        }
    }
}