using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Acorisoft.Miga.Doc.Core
{
    /// <summary>
    /// <see cref="RepositoryStatusEngine"/> 类型表示仓储状态引擎，用于实现响应模式。
    /// </summary>
    public class RepositoryStatusEngine : Disposable
    {
        internal readonly BehaviorSubject<bool> _isEngineLoading;
        private readonly  BehaviorSubject<bool> _waitingQueueStream;

        private int _waitingQueueCount;
        
        public RepositoryStatusEngine()
        {
            _isEngineLoading    = new BehaviorSubject<bool>(false);
            _waitingQueueStream = new BehaviorSubject<bool>(false);

            IsLoaded = _isEngineLoading.AsObservable();
            IsBusy   = _waitingQueueStream.AsObservable();
        }

        internal void Enqueue()
        {
            Interlocked.Increment(ref _waitingQueueCount);
            
            if (_waitingQueueCount == 1)
            {
                _waitingQueueStream.OnNext(true);
            }
        }
        
        internal void Dequeue()
        {
            Interlocked.Decrement(ref _waitingQueueCount);
            
            if (_waitingQueueCount == 0)
            {
                _waitingQueueStream.OnNext(true);
            }
        }

        /// <summary>
        /// 获取或设置当前仓储引擎的加载状态。
        /// </summary>
        public IObservable<bool> IsLoaded { get; }
        public IObservable<bool> IsBusy { get; }
    }
}