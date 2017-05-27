using System.ComponentModel;

namespace WpfApplication1
{
    /// <summary>
    /// abstruct class for viewModels to inherit
    /// </summary>
    abstract class ViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}