using System;
using Microsoft.Win32;
using MoleCC.Helpers;
using MoleCC.Models;
using MoleCC.Views;

namespace MoleCC.ViewModels
{
    class AddVideoViewModel : ViewModelBase
    {
        public RelayCommand OpenVideoPathBrowserCommand { get; set; }
        public RelayCommand OpenSubtitlesPathBrowserCommand { get; set; }
        public RelayCommand SaveVideoCommand { get; set; }

        private string _videoPath;
        public string VideoPath
        {
            get => _videoPath;
            set
            {
                _videoPath = value;
                RaisePropertyChanged("VideoPath");
            }
        }

        private string _subtitlesPath;
        public string SubtitlesPath
        {
            get => _subtitlesPath;
            set
            {
                _subtitlesPath = value;
                RaisePropertyChanged("SubtitlesPath");
            }
        }

        private string _translatedSubtitlesPath;
        public string TranslatedSubtitlesPath
        {
            get => _translatedSubtitlesPath;
            set
            {
                _translatedSubtitlesPath = value;
                RaisePropertyChanged("TranslatedSubtitlesPath");
            }
        }

        public AddVideoViewModel()
        {
            OpenVideoPathBrowserCommand = new RelayCommand(OpenPathBrowser);
            SaveVideoCommand = new RelayCommand(SaveVideo);
            OpenSubtitlesPathBrowserCommand = new RelayCommand(OpenSubtitlesBrowser);
        }

        private void OpenSubtitlesBrowser(object parameter)
        {
            string subtitlesPath = OpenSubtitlesDialog();
            if((string)parameter == "subtitles"){
                SubtitlesPath = subtitlesPath;
            }
            else if((string)parameter == "translated subtitles")
            {
                TranslatedSubtitlesPath = subtitlesPath;
            }
        }

        private string OpenSubtitlesDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Subtitles (*.srt)|*.srt|All files (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return "";
        }

        public void SaveVideo(object parameter)
        {
            //TODO: Save in database
            VideoPlayerWindow videoPlayer = new VideoPlayerWindow();
            ((VideoPlayerViewModel)videoPlayer.DataContext).CurrentVideo = new Video(VideoPath, SubtitlesPath, TranslatedSubtitlesPath);
            videoPlayer.Show();
            CloseAction();
        }

        public void OpenPathBrowser(object parameter)
        {            
            VideoPath = OpenFileDialog();
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
