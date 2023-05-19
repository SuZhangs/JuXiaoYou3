using System;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Inspirations.Pages;

namespace Acorisoft.FutureGL.MigaStudio.Inspirations
{
    public class InspirationControllerBindingProxy : BindingProxy<InspirationController>
    {
        
    }
    
    public class InspirationController : ShellCore
    {       
        protected override void StartOverride()
        {
            RequireStartupTabViewModel();
        }

        protected sealed override void OnStart(RoutingEventArgs arg)
        {
            var p = arg.Parameter;
            GlobalStudioContext = (GlobalStudioContext)p.Args[0];

            if (GlobalStudioContext is null)
            {
                throw new ArgumentNullException(nameof(GlobalStudioContext));
            }
            
            base.OnStart(arg);
        }

        public override void Resume()
        {
            IsCharacterSelected = GlobalStudioContext is not null &&
                                  GlobalStudioContext.Character is not null &&
                                  DocumentEngine
            base.Resume();
        }

        protected override void RequireStartupTabViewModel()
        {
            New<HomeViewModel>();
        }

        private bool _isCharacterSelected;

        public GlobalStudioContext GlobalStudioContext { get; private set; }
        
        /// <summary>
        /// 获取或设置 <see cref="IsCharacterSelected"/> 属性。
        /// </summary>
        public bool IsCharacterSelected
        {
            get => _isCharacterSelected;
            set => SetValue(ref _isCharacterSelected, value);
        }
        
        public override string Id => InspirationSubSystem.Id;
    }
}