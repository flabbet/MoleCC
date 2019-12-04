using Microsoft.Win32;
using MoleCC.Helpers;
using MoleCC.Views;

namespace MoleCC.ViewModels
{
    class AddVideoViewModel : ViewModelBase
    {
        public RelayCommand OpenVideoPathBrowserCommand { get; set; }
        public RelayCommand SaveVideoCommand { get; set; }

        private string _videoFilePath;
        public string VideoFilePath
        {
            get => _videoFilePath;
            set
            {
                _videoFilePath = value;
                RaisePropertyChanged("VideoFilePath");
            }
        }

        public AddVideoViewModel()
        {
            OpenVideoPathBrowserCommand = new RelayCommand(OpenPathBrowser);
            SaveVideoCommand = new RelayCommand(SaveVideo);
        }

        public void SaveVideo(object parameter)
        {
            //TODO: Save in database
            VideoPlayerWindow videoPlayer = new VideoPlayerWindow();
            ((VideoPlayerViewModel)videoPlayer.DataContext).CurrentVideoPath = VideoFilePath;
            videoPlayer.Show();
            CloseAction();
        }

        public void OpenPathBrowser(object parameter)
        {
            VideoFilePath = OpenFileDialog();
        }

        private string OpenFileDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Video files (*.mp4)|*.mp4|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return "";
        }
    }
}
