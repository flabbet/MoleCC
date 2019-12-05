using System;
using System.Collections.Generic;
using System.Text;

namespace MoleCC.Events
{
    public class SetVideoPositionRequestEventArgs : EventArgs
    {
        public TimeSpan Position { get; set; }
        public SetVideoPositionRequestEventArgs(TimeSpan position)
        {
            Position = position;
        }
    }
}
