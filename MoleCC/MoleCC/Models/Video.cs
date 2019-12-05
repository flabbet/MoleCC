using System;
using System.Collections.Generic;
using System.Text;

namespace MoleCC.Models
{
    public class Video
    {
        public string PathToVideo { get; set; }
        public string PathToSubtitles { get; set; }
        public string PathToTranslatedSubititles { get; set; }


        public Video(string pathToVideo, string pathToSubtitles, string pathToTranslatedSubtitles)
        {
            PathToVideo = pathToVideo;
            PathToSubtitles = pathToSubtitles;
            PathToTranslatedSubititles = pathToTranslatedSubtitles;
        }
    }
}
