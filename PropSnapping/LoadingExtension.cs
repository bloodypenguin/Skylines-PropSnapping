using ICities;
using PropSnapping.Redirection;

namespace PropSnapping
{
    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            Redirector<PropInstanceDetour>.Deploy();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            if (mode == LoadMode.LoadGame || mode == LoadMode.NewGame)
            {
                Redirector<PropToolDetour>.Deploy();
            }
            else
            {
                Redirector<PropInstanceDetour>.Revert();
            }
        }

        public override void OnReleased()
        {
            base.OnReleased();
            Redirector<PropToolDetour>.Revert();
            Redirector<PropInstanceDetour>.Revert();
        }
    }
}