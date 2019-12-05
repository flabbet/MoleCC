using System;
using Microsoft.Win32;
using MoleCC.Helpers;
using MoleCC.Models;
using MoleCC.Views;

namespace MoleCC.ViewModels
{
    class AddVideoViewModel : ViewModelBase
    {
        const string SubtitlesFilter = "Subtitles (*.srt)|*.srt|All files (*.*)|*.*";
        const string TranslationDictionaryFilter = "Translation Dictionary (*.trd)|*.trd|All files (*.*)|*.*";
        const string VideoFilter = "Video files (*.mp4)|*.mp4|All files (*.*)|*.*";

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
            if((string)parameter == "subtitles"){
                string subtitlesPath = OpenFileDialog(SubtitlesFilter);
                SubtitlesPath = subtitlesPath;
            }
            else if((string)parameter == "translated subtitles")
            {
                string subtitlesPath = OpenFileDialog(TranslationDictionaryFilter);
                TranslatedSubtitlesPath = subtitlesPath;
            }
        }

        private string OpenFileDialog(string filter)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return "";
        }

        public void SaveVideo(object parameter)
        {
            VideoPlayerWindow videoPlayer = new VideoPlayerWindow();
            ((VideoPlayerViewModel)videoPlayer.DataContext).CurrentVideo = new Video(VideoPath, SubtitlesPath, TranslatedSubtitlesPath);
            ((VideoPlayerViewModel)videoPlayer.DataContext).ParseCurrentVideoSubtitles();
            videoPlayer.Show();
            CloseAction();
        }

        public void OpenPathBrowser(object parameter)
        {            
            VideoPath = OpenFileDialog(VideoFilter);
        }
    }
}
