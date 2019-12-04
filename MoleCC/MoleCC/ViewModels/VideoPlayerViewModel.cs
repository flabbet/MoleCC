using System;
using System.Collections.Generic;
using System.Text;

namespace MoleCC.ViewModels
{
    class VideoPlayerViewModel : ViewModelBase
    {

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

        public VideoPlayerViewModel()
        {

        }
    }
}
