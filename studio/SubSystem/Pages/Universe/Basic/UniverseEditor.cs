using System.Linq;
using System.Windows;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Universe
{
    public class UniverseEditorViewModelProxy : BindingProxy<UniverseEditorBase>
    {
    }

    public abstract partial class UniverseEditorBase : DocumentEditorBase
    {
        protected static void AddSubView<TView>(ICollection<HeaderedSubView> collection, string id, string nc, string hc) where TView : FrameworkElement
        {
            collection.Add(new HeaderedSubView
            {
                Name           = Language.GetText(id),
                Type           = typeof(TView),
                DefaultColor   = nc,
                HighlightColor = hc
            });
        }

        protected override void OnStart()
        {
            CreateSubViews(InternalSubViews);
            SelectedSubView = SubViews.FirstOrDefault();
        }
        
        /// <summary>
        /// 当子页面创建时
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnSubViewChanged(HeaderedSubView oldValue, HeaderedSubView newValue)
        {
            if (newValue is not HeaderedSubView subView)
            {
                return;
            }

            subView.Create(this);
            SubView = subView.SubView;
        }
        
        public sealed override bool Removable => true;
    }
}