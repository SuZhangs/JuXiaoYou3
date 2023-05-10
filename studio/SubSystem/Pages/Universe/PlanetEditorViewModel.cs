namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class PlanetEditorViewModel : UniverseEditorBase
    {
        protected override void CreateSubViews(ICollection<SubViewBase> collection)
        {
            AddSubView<PlanetBasicEditorView>(collection, "text.DocumentEditor.DataPart", "#92A400", "#A1B500");
            AddSubView<PlanetFlowerEditorView>(collection, "text.DocumentEditor.DataPart", "#92A400", "#A1B500");
            AddSubView<PlanetFruitEditorView>(collection, "text.DocumentEditor.DataPart", "#92A400", "#A1B500");
            AddSubView<PlanetRootEditorView>(collection, "text.DocumentEditor.DataPart", "#92A400", "#A1B500");
            AddSubView<PlanetStemEditorView>(collection, "text.DocumentEditor.DataPart", "#92A400", "#A1B500");
            AddSubView<PlanetLeafEditorView>(collection, "text.DocumentEditor.DataPart", "#92A400", "#A1B500");
            AddSubView<PlanetOtherEditorView>(collection, "text.DocumentEditor.DataPart", "#92A400", "#A1B500");
        }

        protected override void OnSubViewChanged(SubViewBase oldValue, SubViewBase newValue)
        {
        }
    }
}