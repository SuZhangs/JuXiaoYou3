using System.Windows.Controls;
using Acorisoft.FutureGL.Forest.Controls.Buttons;

namespace Acorisoft.FutureGL.Forest.Services
{
    public class ForestResolver : LanguageNodeResolver
    {
        public ForestResolver() : base()
        {
            Factory.Add(typeof(HighlightButton), x => new ContentControlResolver { Target = (ContentControl)x });
            Factory.Add(typeof(CallToAction), x => new ContentControlResolver { Target    = (ContentControl)x });
        }
    }
}