using MoleCC.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MoleCC.ViewModels
{
     class ViewModelMain : ViewModelBase
    {
        public RelayCommand AddVideoCommand { get; set; }

        public ViewModelMain()
        {
            AddVideoCommand = new RelayCommand(AddMovie);
        }

        public void AddMovie(object parameter)
        {
            AddVideoWindow window = new AddVideoWindow();
            window.ShowDialog();
        }
    }
}
