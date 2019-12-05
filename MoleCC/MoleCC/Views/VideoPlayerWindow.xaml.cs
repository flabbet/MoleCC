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

namespace MoleCC.Views
{
    /// <summary>
    /// Interaction logic for VideoPlayerWindow.xaml
    /// </summary>
    public partial class VideoPlayerWindow : Window
    {
        public VideoPlayerWindow()
        {
            InitializeComponent();
            VideoPlayerViewModel context = new VideoPlayerViewModel();
            DataContext = context;
            context.player = Player;
            context.window = playerWindow;

            context.RequestPosition += (sender, e) =>
             {
                 Player.Position = e.Position;
             };

            context.PlayRequested += (sender, e) =>
            {
                if (e.PauseVideo)
                {
                    Player.Pause();
                }
                else
                {
                    Player.Play();
                }
            };
            
        }
    }
}
