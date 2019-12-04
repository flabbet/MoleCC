using MoleCC.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MoleCC
{
    /// <summary>
    /// Interaction logic for AddVideoWindow.xaml
    /// </summary>
    public partial class AddVideoWindow : Window
    {
        public AddVideoWindow()
        {
            InitializeComponent();            
            ((ViewModelBase)DataContext).CloseAction = new Action(Close);
        }
    }
}
