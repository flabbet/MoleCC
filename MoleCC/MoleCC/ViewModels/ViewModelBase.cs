using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MoleCC.ViewModels
{
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public Action CloseAction { get; set; }

        internal void RaisePropertyChanged(string property)
        {
            if (property != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        /// <summary>
        /// Use only in view, for closing window in code use CloseAction
        /// </summary>
        /// <param name="parameter"></param>
        internal void CloseWindow(object parameter)
        {
            ((Window)parameter).Close();
        }

        internal void DragMove(object parameter)
        {
            Window popup = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                popup.DragMove();
            }
        }
    }
}
