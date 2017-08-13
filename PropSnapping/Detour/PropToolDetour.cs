using System.Reflection;
using ColossalFramework;
using ColossalFramework.Math;
using PropSnapping.RedirectionFramework.Attributes;
using UnityEngine;

namespace PropSnapping.Detour
{
    [TargetType(typeof(PropTool))]
    public class PropToolDetour : PropTool
    {

//
//
//
//
//        [RedirectMethod]
//        private IEnumerator CreateProp()
//        {
//            UnityEngine.Debug.LogError("CreateProp");
//            if (this.m_placementErrors == ToolBase.ToolErrors.None)
//            {
//                var success__0 = true;
//                var needMoney__1 = (Singleton<ToolManager>.instance.m_properties.m_mode & ItemClass.Availability.Game) != ItemClass.Availability.None;
//                if (needMoney__1)
//                {
//                    var cost__2 = this.m_propInfo.GetConstructionCost();
//                    success__0 = cost__2 == 0 || cost__2 == Singleton<EconomyManager>.instance.FetchResource(EconomyManager.Resource.Construction, cost__2, this.m_propInfo.m_class);
//                }
//                ushort prop__3;
//                Randomizer randomizer = m_randomizer;
//                if (success__0 && Singleton<PropManager>.instance.CreateProp(out prop__3, ref randomizer, this.m_propInfo, m_mousePosition, this.m_angle * ((float)System.Math.PI / 180f), true))
//                {
//                    //begin mod
//                    Singleton<PropManager>.instance.m_props.m_buffer[prop__3].FixedHeight = true;
//                    //end mod
//                    this.m_propInfo = this.m_prefab.GetVariation(ref randomizer);
//                    PropTool.DispatchPlacementEffect(m_mousePosition, false);
//                }
//                m_randomizer = randomizer;
//            }
//            yield return null;
//        }



        [RedirectMethod]
        public override void SimulationStep()
        {
            if (this.m_prefab == null)
            {
                this.m_wasPrefab = (PropInfo)null;
                this.m_propInfo = (PropInfo)null;
            }
            else
            {
                if (this.m_propInfo == null || this.m_wasPrefab != this.m_prefab)
                {
                    this.m_wasPrefab = this.m_prefab;
                    Randomizer r = this.m_randomizer;
                    this.m_propInfo = (Singleton<ToolManager>.instance.m_properties.m_mode & ItemClass.Availability.AssetEditor) == ItemClass.Availability.None ? this.m_prefab.GetVariation(ref r) : this.m_prefab;
                    m_randomizer = this.m_randomizer;
                }
                ToolBase.RaycastInput input = new ToolBase.RaycastInput(this.m_mouseRay, this.m_mouseRayLength);
                //begin mod
                input.m_ignoreBuildingFlags = Building.Flags.None;
                input.m_ignoreNodeFlags = NetNode.Flags.None;
                input.m_ignoreSegmentFlags = NetSegment.Flags.None;
                input.m_buildingService = new RaycastService(ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Layer.Default);
                input.m_netService = new RaycastService(ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Layer.Default);
                input.m_netService2 = new RaycastService(ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Layer.Default);
                //end mod
                ulong[] collidingSegments;
                ulong[] collidingBuildings;
                this.m_toolController.BeginColliding(out collidingSegments, out collidingBuildings);
                try
                {
                    ToolBase.RaycastOutput output;
                    if (this.m_mouseRayValid && ToolBase.RayCast(input, out output))
                    {
                        if (this.m_mode == PropTool.Mode.Brush)
                        {
                            this.m_mousePosition = output.m_hitPos;
                            this.m_placementErrors = !Singleton<PropManager>.instance.CheckLimits()
                                ? ToolBase.ToolErrors.TooManyObjects
                                : ToolBase.ToolErrors.Pending;
                            if (this.m_mouseLeftDown == this.m_mouseRightDown)
                                return;
                            this.ApplyBrush();
                        }
                        else
                        {
                            if (this.m_mode != PropTool.Mode.Single)
                                return;
                            //begin mod
                            //end mod
                            Randomizer r = this.m_randomizer;
                            ushort id = Singleton<PropManager>.instance.m_props.NextFreeItem(ref r);
                            ToolBase.ToolErrors toolErrors = PropTool.CheckPlacementErrors(this.m_propInfo,
                                output.m_hitPos, /*output.m_currentEditObject*/false, id, collidingSegments,
                                collidingBuildings);
                            if ((Singleton<ToolManager>.instance.m_properties.m_mode & ItemClass.Availability.Game) !=
                                ItemClass.Availability.None)
                            {
                                int constructionCost = this.m_propInfo.GetConstructionCost();
                                if (constructionCost != 0 &&
                                    constructionCost !=
                                    Singleton<EconomyManager>.instance.PeekResource(
                                        EconomyManager.Resource.Construction, constructionCost))
                                    toolErrors |= ToolBase.ToolErrors.NotEnoughMoney;
                            }
                            if (!Singleton<PropManager>.instance.CheckLimits())
                                toolErrors |= ToolBase.ToolErrors.TooManyObjects;
                            this.m_mousePosition = output.m_hitPos;
                            this.m_placementErrors = toolErrors;
                            //begin mod
                            //end mod
                        }
                    }
                    else
                        this.m_placementErrors = ToolBase.ToolErrors.RaycastFailed;
                }
                finally
                {
                    this.m_toolController.EndColliding();
                }
            }
        }

        private ToolBase.ToolErrors m_placementErrors
        {
            set
            {
                typeof(PropTool).GetField("m_placementErrors", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(this, value);
            }
            get
            {
                return (ToolBase.ToolErrors)typeof(PropTool).GetField("m_placementErrors", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);
            }
        }

        //struct
        private Vector3 m_mousePosition
        {
            set
            {
                typeof(PropTool).GetField("m_mousePosition", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(this, value);
            }
            get
            {
                return (Vector3)typeof(PropTool).GetField("m_mousePosition", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);
            }
        }

        private PropInfo m_propInfo
        {

            get
            {
                return (PropInfo)typeof(PropTool).GetField("m_propInfo", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);
            }
            set
            {
                typeof(PropTool).GetField("m_propInfo", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(this, value);
            }
        }

        //struct
        private Randomizer m_randomizer
        {

            get
            {
                return (Randomizer)typeof(PropTool).GetField("m_randomizer", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);
            }
            set
            {
                typeof(PropTool).GetField("m_randomizer", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(this, value);
            }
        }

        private PropInfo m_wasPrefab
        {

            get
            {
                return (PropInfo)typeof(PropTool).GetField("m_wasPrefab", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);
            }
            set
            {
                typeof(PropTool).GetField("m_wasPrefab", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(this, value);
            }
        }

        private bool m_mouseRayValid => (bool)typeof(PropTool).GetField("m_mouseRayValid", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(this);

        private bool m_mouseLeftDown => (bool)typeof(PropTool).GetField("m_mouseLeftDown", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(this);

        private bool m_mouseRightDown => (bool)typeof(PropTool).GetField("m_mouseRightDown", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(this);

        private float m_mouseRayLength => (float)typeof(PropTool).GetField("m_mouseRayLength", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(this);

        private Ray m_mouseRay => (Ray)typeof(PropTool).GetField("m_mouseRay", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(this);

        [RedirectReverse]
        private void ApplyBrush()
        {
            UnityEngine.Debug.LogError("Failed to redirect ApplyBrush()");
        }
    }
}
