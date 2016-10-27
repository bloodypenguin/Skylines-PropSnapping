using ICities;
using PropSnapping.Redirection;

namespace PropSnapping
{
    public class LoadingExctension : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
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