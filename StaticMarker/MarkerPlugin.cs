using NLog;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.IO;
using System.Windows.Controls;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Plugins;
using Torch.API.Session;
using Torch.Session;

namespace StaticMarker
{
    public class MarkerPlugin : TorchPluginBase, IWpfPlugin
    {
        const string ConfigFile = "StaticMarkerEntries.cfg";
        const string ConfigMarkersFile = "StaticMarkerEntries.cfg";

        private MarkerControl _control;
        private TorchSessionManager _sessionManager;
        private IMultiplayerManagerBase _multibase;
        private Persistent<MarkerConfig> _config;
        private MarkerEntriesConfig _entriesConfig;

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public MarkerConfig Config => _config?.Data;

        /// <inheritdoc />
        public UserControl GetControl() => _control ?? (_control = new MarkerControl(this));

        public void Save() => _config.Save();

        /// <inheritdoc />
        public override void Init(ITorchBase torch) 
        {
            base.Init(torch);

            _sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (_sessionManager != null)
                _sessionManager.SessionStateChanged += SessionChanged;
            else
                Log.Warn("No session manager loaded!");

            var configFile = Path.Combine(StoragePath, ConfigFile);
            try
            {
                _config = Persistent<MarkerConfig>.Load(configFile);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }

            if (_config?.Data == null)
                _config = new Persistent<MarkerConfig>(configFile, new MarkerConfig());

            LoadConfig();
        }

        private void SessionChanged(ITorchSession session, TorchSessionState state)
        {
            switch (state)
            {
                case TorchSessionState.Loaded:
                    _multibase = Torch.CurrentSession.Managers.GetManager<IMultiplayerManagerBase>();
                    if (_multibase != null)
                        _multibase.PlayerJoined += _multibase_PlayerJoined;
                    else
                        Log.Warn("No multiplayer manager loaded!");
                    break;
                case TorchSessionState.Unloading:
                    if (_multibase != null)
                        _multibase.PlayerJoined -= _multibase_PlayerJoined;
                    break;
            }
        }

        internal bool LoadConfig()
        {
            var success = false;
            var configFile = Path.Combine(StoragePath, ConfigMarkersFile);
            try
            {
                _entriesConfig = Persistent<MarkerEntriesConfig>.Load(configFile).Data;
                success = true;
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }

            if (_config?.Data == null)
                _entriesConfig = new Persistent<MarkerEntriesConfig>(configFile, new MarkerEntriesConfig()).Data;

            return success;
        }

        private void _multibase_PlayerJoined(IPlayer obj)
        {
            Log.Info(obj.State.ToString());
            var idendity = MySession.Static.Players.TryGetIdentityId(obj.SteamId);
            if (idendity == 0)
            {
                Log.Info("Identity not found");
                return;
            }

            if (Config.SendMarkerOnJoin)
                SendGPSEntries(idendity);
        }

        internal void SendGPSEntries(long identityId)
        {
            foreach (var entry in _entriesConfig.Entries)
            {
                MyAPIGateway.Session?.GPS.AddGps(identityId, new MyGps(entry));
            }
        }
    }
}
