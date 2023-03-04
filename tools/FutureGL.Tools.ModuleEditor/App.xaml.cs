using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Acorisoft.FutureGL.Tools.ModuleEditor.ViewModels;
using DryIoc;
using NLog;

namespace Acorisoft.FutureGL.Tools.ModuleEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void RegisterServices(ILogger logger, IContainer container)
        {
            throw new NotImplementedException();
        }

        protected override void RegisterViews(ILogger logger, IContainer container)
        {
            throw new NotImplementedException();
        }

        protected override AppViewModel GetShell()
        {
            throw new NotImplementedException();
        }
    }
}