﻿using Client.Main.Controls;
using Microsoft.Xna.Framework;

namespace Client.Main.Worlds
{
    public class SelectCharacterWorld : WorldControl
    {
        public SelectCharacterWorld() : base(worldIndex: 75)
        {
            Camera.Instance.ViewFar = 5000f;
            Camera.Instance.Position = new Vector3(9858, 18813, 700);
            Camera.Instance.Target = new Vector3(7200, 19550, 550);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }
    }
}
