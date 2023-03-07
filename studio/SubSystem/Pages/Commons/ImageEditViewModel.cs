
using Acorisoft.FutureGL.Forest.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Commons
{
    public class ImageEditViewModel : InputViewModel
    {
        public override void Start()
        {
            base.Start();
        }

        protected override void OnStart(Parameter parameter)
        {
            if (parameter.Args[0] is Image<Rgba32> img)
            {
                
            }
        }

        protected override void ReleaseManagedResources()
        {
            base.ReleaseManagedResources();
        }

        protected override bool IsCompleted()
        {
            throw new System.NotImplementedException();
        }

        protected override void Finish()
        {
            throw new System.NotImplementedException();
        }

        protected override string Failed()
        {
            throw new System.NotImplementedException();
        }
    }
}