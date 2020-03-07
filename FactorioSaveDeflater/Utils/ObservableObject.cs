using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FactorioSaveDeflater.Utils
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetAndNotify<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(backingField, value))
            {
                return false;
            }

            backingField = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        protected void Raise([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
