namespace Acorisoft.FutureGL.MigaUtils
{
    public class Disposable : IDisposable
    {
        protected virtual void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        protected virtual void ReleaseManagedResources()
        {
            // TODO release managed resources here
        }

        protected void Dispose(bool disposing)
        {
            ReleaseManagedResources();
            if (disposing)
            {
                ReleaseUnmanagedResources();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Disposable()
        {
            Dispose(false);
        }
    }
}