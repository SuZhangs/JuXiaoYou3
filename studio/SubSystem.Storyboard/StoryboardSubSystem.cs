using System;
using System.Collections.Generic;
using Acorisoft.FutureGL.Forest.AppModels;
using Acorisoft.FutureGL.Forest.Services;
using Acorisoft.FutureGL.MigaStudio.Core;
using DryIoc;

namespace Acorisoft.FutureGL.MigaStudio.Storyboard
{
    public class StoryboardSubSystem : SubSystemModule
    {
        public override ITabViewController GetController() => new StoryboardController();

        protected override string GetSubSystemName(CultureArea language)
        {
            return language switch
            {
                _ => "灵感模式"
            };
        }

        protected override string GetLanguageFilePrefix() => "Acorisoft.FutureGL.MigaStudio.Storyboard.Languages.";

        protected override void InstallView(IContainer container)
        {
        }

        protected override void InstallServices(IContainer container)
        {
        }

        public override string ModuleId => Id;
        public const string Id = "__Storyboard";
    }
}