using System.Reactive.Concurrency;
using Acorisoft.FutureGL.MigaStudio.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.Models
{
    public class AsyncJob : ObservableObject
    {
        /// <summary>
        /// 
        /// </summary>
        public Action<TabBaseAppViewModel> Handler { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; init; }
    }
}