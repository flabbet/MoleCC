using MoleCC.Events;
using MoleCC.Helpers;
using MoleCC.Models;
using SubtitlesParser.Classes;
using SubtitlesParser.Classes.Parsers;
using System;
using System.Collections.Generic;
using System.IO;

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

        private string _currentSubtitles;
        public string CurrentSubtitles
        {
            get => _currentSubtitles;
            set
            {
                _currentSubtitles = value;
                RaisePropertyChanged("CurrentSubtitles");
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

        private List<SubtitleItem> _parsedSubtitles;

        public RelayCommand PlayOrStopVideoCommand { get; set; }
        public RelayCommand PlayVideoCommand { get; set; }


        public VideoPlayerViewModel()
        {
            PlayOrStopVideoCommand = new RelayCommand(PlayOrStopVideo);
            PlayVideoCommand = new RelayCommand(PlayVideo);
            _parsedSubtitles = ParseSubtitles(CurrentVideo.PathToSubtitles);
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

        public List<SubtitleItem> ParseSubtitles(string pathToFile)
        {
            var parser = new SubParser();
            using (var fileStream = File.OpenRead(pathToFile))
            {
                return parser.ParseStream(fileStream);
            }
        }
    }
}
