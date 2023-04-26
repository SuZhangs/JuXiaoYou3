using System;
using System.Collections.Generic;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using DryIoc;

namespace Acorisoft.FutureGL.MigaStudio.Composer
{
    public class ComposerSubSystem : SubSystemModule
    {
        public override ITabViewController GetController() => new ComposerController();

        protected override string GetSubSystemName(CultureArea language)
        {
            return language switch
            {
                _ => "建造模式"
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
        public const string Id = "__Composer";
    }
}