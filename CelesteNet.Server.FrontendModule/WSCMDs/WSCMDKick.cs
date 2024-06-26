﻿using Celeste.Mod.CelesteNet.DataTypes;
using Celeste.Mod.CelesteNet.Server.Chat;
using MonoMod.Utils;

namespace Celeste.Mod.CelesteNet.Server.Control
{
    public class WSCMDKick : WSCMD<uint> {
        public override bool MustAuth => true;
        public override object? Run(uint input) {
            if (!Frontend.Server.PlayersByID.TryGetValue(input, out CelesteNetPlayerSession? player))
                return false;

            Logger.Log(LogLevel.VVV, "frontend", $"Kick called: {input} => {player.UID}");

            ChatModule chat = Frontend.Server.Get<ChatModule>();
            new DynamicData(player).Set("leaveReason", chat.Settings.MessageKick);
            player.Con.Send(new DataDisconnectReason { Text = chat.Settings.MessageDefaultKickReason });
            player.Con.Send(new DataInternalDisconnect());
            player.Dispose();
            return true;
        }
    }
}
