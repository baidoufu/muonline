﻿using Client.Main.Controls;
using Client.Main.Core.Utilities;
using Microsoft.Xna.Framework;

namespace Client.Main.Worlds
{
    [WorldInfo(50, "Elbeland 2")]
    public class World051World : WalkableWorldControl
    {
        public World051World() : base(worldIndex: 51)
        {

        }

        public override void AfterLoad()
        {
            Vector2 defaultSpawn = new Vector2(170, 185);
            Walker.Reset();
            bool shouldUseDefaultSpawn = false;
            if (MuGame.Network == null ||
                MuGame.Network.CurrentState == Core.Client.ClientConnectionState.Initial ||
                MuGame.Network.CurrentState == Core.Client.ClientConnectionState.Disconnected)
            {
                shouldUseDefaultSpawn = true;
            }
            else if (Walker.Location == Vector2.Zero)
            {
                shouldUseDefaultSpawn = true;
            }
            if (shouldUseDefaultSpawn)
            {
                Walker.Location = defaultSpawn;
            }
            Walker.MoveTargetPosition = Walker.TargetPosition;
            Walker.Position = Walker.TargetPosition;
            
            base.AfterLoad();
        }
    }
}
