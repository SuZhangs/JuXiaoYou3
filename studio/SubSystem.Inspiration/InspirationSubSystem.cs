using System;
using System.Collections.Generic;
using System.IO;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.MigaStudio.Core;
using DryIoc;

namespace Acorisoft.FutureGL.MigaStudio.Inspirations
{
    public class InspirationSubSystem : SubSystemModule
    {
        
        public override ITabViewController GetController() => new InspirationController();
        protected override string GetSubSystemName(CultureArea language)
        {
            return language switch
            {
                _ => "灵感模式"
            };
        }

        protected override IEnumerable<string> InstallLanguages(CultureArea culture)
        {
            return Array.Empty<string>();
        }

        protected override void InstallView(IContainer container)
        {
            
        }

        protected override void InstallServices(IContainer container)
        {
        }

        public override string ModuleId => Id;
        public const string Id = "__Inspiration";
    }
}