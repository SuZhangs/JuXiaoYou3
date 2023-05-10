namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class PlanetEditorViewModel : UniverseEditorBase
    {
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddSubView<PlanetBasicEditorView>(collection, "text.Planet.Basic", "#92A400", "#A1B500");
            AddSubView<PlanetFlowerEditorView>(collection, "text.Planet.Flower", "#92A400", "#A1B500");
            AddSubView<PlanetFruitEditorView>(collection, "text.Planet.Fruit", "#92A400", "#A1B500");
            AddSubView<PlanetRootEditorView>(collection, "text.Planet.Root", "#92A400", "#A1B500");
            AddSubView<PlanetStemEditorView>(collection, "text.Planet.Stem", "#92A400", "#A1B500");
            AddSubView<PlanetLeafEditorView>(collection, "text.Planet.Leaf", "#92A400", "#A1B500");
            AddSubView<PlanetOtherEditorView>(collection, "global.Other", "#92A400", "#A1B500");
        }
    }
}