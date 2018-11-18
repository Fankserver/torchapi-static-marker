using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace StaticMarker
{
    public class MarkerCommands : CommandModule
    {
        public MarkerPlugin Plugin => (MarkerPlugin)Context.Plugin;

        [Command("gps", "Show static gps marker")]
        [Permission(MyPromoteLevel.None)]
        public void GPS()
        {
            if (!(Context?.Player?.SteamUserId > 0))
            {
                Context.Respond("Command can be used ingame only");
                return;
            }

            Plugin.SendGPSEntries(Context.Player.IdentityId);
        }

        [Command("gps reload", "Reload static gps marker from config file")]
        [Permission(MyPromoteLevel.Admin)]
        public void GPSReload()
        {
            if (Plugin.LoadConfig())
                Context.Respond("success");
            else
                Context.Respond("error");
        }
    }
}
