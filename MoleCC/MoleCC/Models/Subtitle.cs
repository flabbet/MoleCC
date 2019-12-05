using System;
using System.Collections.Generic;
using System.Text;

namespace MoleCC.Models
{
    public struct Subtitle
    {
        public string OriginalSubitile { get; set; }
        public string Translation { get; set; }

        public Subtitle(string originalSubtitle, string translation)
        {
            OriginalSubitile = originalSubtitle;
            Translation = translation;
        }
    }
}
