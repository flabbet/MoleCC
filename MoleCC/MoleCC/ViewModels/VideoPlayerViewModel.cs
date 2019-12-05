using MoleCC.Events;
using MoleCC.Helpers;
using MoleCC.Models;
using SubtitlesParser.Classes;
using SubtitlesParser.Classes.Parsers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MoleCC.ViewModels
{
    class VideoPlayerViewModel : ViewModelBase
    {
        public event EventHandler<PlayVideoRequestEventArgs> PlayRequested;
        public event EventHandler<SetVideoPositionRequestEventArgs> RequestPosition;
        public MediaElement player; //I know this is violation of MVVM, but it is hard to work with MediaElement using MVVM
        public Window window;

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

        public ObservableCollection<Subtitle> Subtitles { get; set; }

        private string _translationForSelectedWord;
        public string TranslationForSelectedWord
        {
            get => _translationForSelectedWord;
            set
            {
                _translationForSelectedWord = value;
                RaisePropertyChanged("TranslationForSelectedWord");
            }
        }

        private string _videoTime;
        public string VideoTime
        {
            get => _videoTime;
            set
            {
                _videoTime = value;
                RaisePropertyChanged("VideoTime");
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

        private double _videoPosition;
        public double VideoPosition
        {
            get => _videoPosition;
            set
            {
                _videoPosition = value;
                RaisePropertyChanged("VideoPosition");
                RequestPosition?.Invoke(this, new SetVideoPositionRequestEventArgs(new TimeSpan((long)(player.NaturalDuration.TimeSpan.Ticks * value))));
            }
        }

        private bool _showPopup = false;
        public bool ShowPopup
        {
            get => _showPopup;
            set
            {
                _showPopup = value;
                RaisePropertyChanged("ShowPopup");
            }
        }

        private double _popupHoriozntalOffset;
        public double PopupHorizontalOffset
        {
            get => _popupHoriozntalOffset;
            set
            {
                _popupHoriozntalOffset = value;
                RaisePropertyChanged("PopupHorizontalOffset");
            }
        }

        private double _popupVerticallOffset;
        public double PopupVerticalOffset
        {
            get => _popupVerticallOffset;
            set
            {
                _popupVerticallOffset = value;
                RaisePropertyChanged("PopupVerticalOffset");
            }
        }

        public bool IsVideoPlaying { get; private set; } = false;

        private List<SubtitleItem> _parsedSubtitles;
        private List<Dictionary<string, string>> _parsedTranslation;
        private int _currentTranslationIndex;

        public RelayCommand PlayOrStopVideoCommand { get; set; }
        public RelayCommand PlayVideoCommand { get; set; }
        public RelayCommand TranslateSubtitleCommand { get; set; }


        public VideoPlayerViewModel()
        {
            PlayOrStopVideoCommand = new RelayCommand(PlayOrStopVideo);
            PlayVideoCommand = new RelayCommand(PlayVideo);
            TranslateSubtitleCommand = new RelayCommand(ShowTranslation);
            Subtitles = new ObservableCollection<Subtitle>();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void ParseCurrentVideoSubtitles()
        {
            if (CurrentVideo.PathToSubtitles != null)
            {
                _parsedSubtitles = ParseSubtitles(CurrentVideo.PathToSubtitles);
            }
            if (CurrentVideo.PathToTranslatedSubititles != null)
            {
                _parsedTranslation = ParseTranslation(CurrentVideo.PathToTranslatedSubititles);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            GenerateSubtitles();
            VideoPosition = (100 * player.Position.Ticks / (double)player.NaturalDuration.TimeSpan.Ticks) / 100;
            VideoTime = $"{player.Position.ToString(@"mm\:ss")} \\ {player.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}";
        }

        private void GenerateSubtitles()
        {
            if (_parsedSubtitles != null)
            {
                int itemIndex = _parsedSubtitles.FindIndex(x => x.StartTime >= player.Position.TotalMilliseconds && x.EndTime > player.Position.TotalMilliseconds);
                if (itemIndex > _parsedSubtitles.Count - 1 || itemIndex > _parsedTranslation.Count - 1 || itemIndex < 0 ) return;
                SubtitleItem item = _parsedSubtitles[itemIndex];
                Dictionary<string, string> translations = _parsedTranslation[itemIndex];
                Subtitles.Clear();
                _currentTranslationIndex = itemIndex;
                for (int i = 0; i < translations.Count; i++)
                {
                    var translation = translations.ElementAt(i);
                    Subtitles.Add(new Subtitle(translation.Key, translation.Value));
                }
            }
        }

        public void ShowTranslation(object parameter)
        {
            if (Mouse.DirectlyOver.GetType() == typeof(TextBlock) && ((TextBlock)Mouse.DirectlyOver).Name != "translationPopup")
            {
                TranslationForSelectedWord = _parsedTranslation.ElementAt(_currentTranslationIndex).GetValueOrDefault(((TextBlock)Mouse.DirectlyOver).Text);
                ShowPopup = true;
                var mousePos = Mouse.GetPosition(window);
                PopupVerticalOffset = mousePos.Y + 10;
                PopupHorizontalOffset = mousePos.X + 10;
            }
            else
            {
                ShowPopup = false;
            }
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

        public void SetVideoPosition(object parameter)
        {

        }

        public List<Dictionary<string, string>> ParseTranslation(string pathToTrasnlation)
        {
            var parser = new TrdTranslationParser();
            using (var fileStream = File.OpenRead(pathToTrasnlation))
            {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    return parser.ParseStream(streamReader);
                }
            }
        }
    }
}
