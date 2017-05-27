using System.ComponentModel;

namespace WpfApplication1
{
    /// <summary>
    /// Abstract class for a view-model (VM) of the MVVM pattern.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    abstract class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property have changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}