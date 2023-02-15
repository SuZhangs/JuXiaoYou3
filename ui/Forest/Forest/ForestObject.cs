using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Acorisoft.FutureGL.Forest
{
    /// <summary>
    /// <see cref="ForestObject"/> 类型表示一个可绑定的UI对象。
    /// </summary>
    public abstract class ForestObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private PropertyChangedEventHandler PropertyChangedHandler;
        private PropertyChangingEventHandler PropertyChangingHandler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected internal void RaiseUpdated([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName)) return;
            PropertyChangedHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaiseUpdating([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName)) return;
            PropertyChangingHandler?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }
        
        /// <summary>
        /// 释放非托管资源
        /// </summary>
        protected virtual void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        /// <summary>
        /// 释放托管资源
        /// </summary>
        protected virtual void ReleaseManagedResources()
        {
            // TODO release managed resources here
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected void Dispose(bool disposing)
        {
            ReleaseManagedResources();
            if (disposing)
            {
                ReleaseUnmanagedResources();
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
#pragma warning disable CA1816
            GC.SuppressFinalize(this);
#pragma warning restore CA1816
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected bool SetValue<T>(ref T source, T value, [CallerMemberName] string name = "")
        {
            if (string.IsNullOrEmpty(name)) return false;
            
            if (EqualityComparer<T>.Default.Equals(source, value))
            {
                return false;
            }

            RaiseUpdating(name);
            source = value;
            RaiseUpdated(name);
            return true;
        }
        
        public event PropertyChangedEventHandler PropertyChanged
        {
            add => PropertyChangedHandler += value;
            remove => PropertyChangedHandler -= value;
        }
        
        public event PropertyChangingEventHandler PropertyChanging
        {
            add => PropertyChangingHandler += value;
            remove => PropertyChangingHandler -= value;
        }

        ~ForestObject()
        {
            Dispose(false);
        }
    }
}