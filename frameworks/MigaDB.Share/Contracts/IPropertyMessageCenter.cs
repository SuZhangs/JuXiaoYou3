namespace Acorisoft.FutureGL.MigaDB.Contracts
{
    public interface IPropertyMessageCenter
    {
        void ForceValueChanged();
        void ForceValueChangedAndClamp();
    }
}