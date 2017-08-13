using ICities;
using PropSnapping.Detour;
using PropSnapping.RedirectionFramework;

namespace PropSnapping
{
    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            if (loading.currentMode != AppMode.Game)
            {
                return;
            }
            Redirector<PropToolDetour>.Deploy();
            Redirector<PropInstanceDetour>.Deploy();
        }

        public override void OnReleased()
        {
            base.OnReleased();
            Redirector<PropToolDetour>.Revert();
            Redirector<PropInstanceDetour>.Revert();
        }
    }
}