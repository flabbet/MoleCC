using MoleCC.Events;
using MoleCC.Helpers;
using MoleCC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoleCC.ViewModels
{
    class VideoPlayerViewModel : ViewModelBase
    {
        public event EventHandler<PlayVideoRequestEventArgs> PlayRequested;

        private Video _currentVideo;
        public Video CurrentVideo 
        {
            get => _currentVideo;
            set
            {
                _currentVideo = value;
                RaisePropertyChanged("CurrentVideo");
            }
        }

        private double _volume = 100;
        public double Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                RaisePropertyChanged("Volume");
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
