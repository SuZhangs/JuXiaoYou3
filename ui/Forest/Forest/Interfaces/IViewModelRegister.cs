
using System.Collections.Generic;
using Acorisoft.FutureGL.Forest.Models;
using DryIoc;

namespace Acorisoft.FutureGL.Forest.Interfaces
{
    public interface IViewModelRegister
    {
        void Register(IViewInstaller container);
    }

    public interface IViewInstaller
    {
        void Install(BindingInfo info);
        void Install(IEnumerable<BindingInfo> bindingInfos);
        void Install(IBindingInfoProvider provider);
        void Install(IEnumerable<IBindingInfoProvider> providers);
    }
}