﻿using Microsoft.Xna.Framework.Graphics;

namespace Client.Data.Texture
{
    public class TextureData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public SurfaceFormat Format { get; set; }
        public byte Components { get; set; }
        public bool IsCompressed { get; set; }
        public byte[] Data { get; set; } = [];
    }
}