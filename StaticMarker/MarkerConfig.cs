using Torch;

namespace StaticMarker
{
    public class MarkerConfig : ViewModel
    {
        private bool _sendMarkerOnJoin = true;
        public bool SendMarkerOnJoin { get => _sendMarkerOnJoin; set => SetValue(ref _sendMarkerOnJoin, value); }
    }
}
