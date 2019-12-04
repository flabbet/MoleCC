using MoleCC.Events;
using MoleCC.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoleCC.ViewModels
{
    class VideoPlayerViewModel : ViewModelBase
    {
        public event EventHandler<PlayVideoRequestEventArgs> PlayRequested;

        private string _currentVideoPath;
        public string CurrentVideoPath 
        {
            get => _currentVideoPath;
            set
            {
                _currentVideoPath = value;
                RaisePropertyChanged("CurrentVideoPath");
            }
        }

        public bool IsVideoPlaying { get; private set; } = false;

        public RelayCommand PlayOrStopVideoCommand { get; set; }
        public RelayCommand PlayVideoCommand { get; set; }


        public VideoPlayerViewModel()
        {
            PlayOrStopVideoCommand = new RelayCommand(PlayOrStopVideo);
            PlayVideoCommand = new RelayCommand(PlayVideo);
        }

        public void PlayVideo(object parameter)
        {
            PlayRequested?.Invoke(this, new PlayVideoRequestEventArgs() { PauseVideo = false });
        }

        public void PlayOrStopVideo(object parameter)
        {
            IsVideoPlaying = !IsVideoPlaying;
            PlayRequested?.Invoke(this, new PlayVideoRequestEventArgs() { PauseVideo = IsVideoPlaying });
        }
    }
}
