using System.Threading.Tasks;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons.Dialogs
{
    public class EditPlainTextViewModel : DialogViewModel
    {
        public static Task<Op<object>> Edit(StickyNote note)
        {
            if (note is null)
            {
                return Task.FromResult(Op<object>.Failed("参数为空"));
            }

            return DialogService().Dialog(new EditPlainTextViewModel(), new Parameter
            {
                Args = new[] { note }
            });
        }
    }
}