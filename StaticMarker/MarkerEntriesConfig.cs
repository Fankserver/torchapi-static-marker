using System.Collections.Generic;
using VRage.Game;

namespace StaticMarker
{
    public class MarkerEntriesConfig
    {
        private List<MyObjectBuilder_Gps.Entry> _entries = new List<MyObjectBuilder_Gps.Entry>();
        public List<MyObjectBuilder_Gps.Entry> Entries {
            get => _entries;
            set {
                _entries.Clear();
                _entries.AddRange(value);
            }
        }
    }
}
