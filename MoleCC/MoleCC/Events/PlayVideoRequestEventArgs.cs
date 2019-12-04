using System;
using System.Collections.Generic;
using System.Text;

namespace MoleCC.Events
{
    public class PlayVideoRequestEventArgs : EventArgs
    {
        public bool PauseVideo { get; set; }
    }
}
