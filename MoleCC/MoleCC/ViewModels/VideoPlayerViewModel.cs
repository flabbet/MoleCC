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
using System.Windows.Threading;

namespace MoleCC.ViewModels
{
    class VideoPlayerViewModel : ViewModelBase
    {
        public event EventHandler<PlayVideoRequestEventArgs> PlayRequested;
        public MediaElement player; //I know this is violation of MVVM, but it is hard to work with MediaElement using MVVM

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
        private List<Dictionary<string, string>> _parsedTranslation;

        public RelayCommand PlayOrStopVideoCommand { get; set; }
        public RelayCommand PlayVideoCommand { get; set; }


        public VideoPlayerViewModel()
        {
            PlayOrStopVideoCommand = new RelayCommand(PlayOrStopVideo);
            PlayVideoCommand = new RelayCommand(PlayVideo);
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
            GenerateSubitles();
        }

        private void GenerateSubitles()
        {
            if (_parsedSubtitles != null)
            {
                int itemIndex = _parsedSubtitles.FindIndex(x => x.StartTime >= player.Position.TotalMilliseconds && x.EndTime > player.Position.TotalMilliseconds);
                if (itemIndex > _parsedSubtitles.Count - 1 || itemIndex > _parsedTranslation.Count - 1) return;
                SubtitleItem item = _parsedSubtitles[itemIndex];
                Dictionary<string, string> translations = _parsedTranslation[itemIndex];
                Subtitles.Clear();
                for (int i = 0; i < translations.Count; i++)
                {
                    var translation = translations.ElementAt(i);
                    Subtitles.Add(new Subtitle(translation.Key, translation.Value));
                }
            }
        }

        public void ShowTranslation(object parameter)
        {
            TranslationForSelectedWord = (string)parameter;
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
