using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch;

namespace StaticMarker
{
    public class MarkerConfig : ViewModel
    {
        private bool _sendMarkerOnJoin = true;
        public bool SendMarkerOnJoin { get => _sendMarkerOnJoin; set => SetValue(ref _sendMarkerOnJoin, value); }
    }
}
